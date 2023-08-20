﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Package_Images
{
    public partial class Main : Form
    {
        private readonly int MaxResults = 500;
        private int TotalMessages = 0;
        private List<DChannel> Channels = new List<DChannel>();
        private DateTime PackageCreationTime;
        
        public static DUser User;
        public static List<DAttachment> AllAttachments = new List<DAttachment>();
        public static List<DAnalyticsGuild> AllJoinedGuilds = new List<DAnalyticsGuild>();
        public static dynamic CurrentGuilds;
        public static string AccountToken = "";
        public Main()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if(Properties.Settings.Default.DeletedMessageIDs == null)
            {
                Properties.Settings.Default.DeletedMessageIDs = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }

            switch(Properties.Settings.Default.UseDiscordInstance)
            {
                case "default":
                    defaultRb.Checked = true;
                    break;
                case "stable":
                    stableRb.Checked = true;
                    break;
                case "ptb":
                    ptbRb.Checked = true;
                    break;
                case "canary":
                    canaryRb.Checked = true;
                    break;
                case "web_stable":
                    webStableRb.Checked = true;
                    break;
                case "web_ptb":
                    webPTBRb.Checked = true;
                    break;
                case "web_canary":
                    webCanaryRb.Checked = true;
                    break;
            }
        }

        public static void LaunchDiscordProtocol(string url)
        {
            string instance = Properties.Settings.Default.UseDiscordInstance;
            if(instance == "default")
            {
                Process.Start($"discord://-/{url}");
                return;
            }

            if(instance.StartsWith("web_"))
            {
                string hostname;
                switch (instance)
                {
                    case "web_stable":
                        hostname = "discord.com";
                        break;
                    case "web_ptb":
                        hostname = "ptb.discord.com";
                        break;
                    case "web_canary":
                        hostname = "canary.discord.com";
                        break;
                    default:
                        throw new Exception($"Invalid settings value: {instance}");
                }

                Process.Start($"https://{hostname}/{url}");
                return;
            }

            string folderName;
            switch(instance)
            {
                case "stable":
                    folderName = "Discord";
                    break;
                case "ptb":
                    folderName = "DiscordPTB";
                    break;
                case "canary":
                    folderName = "DiscordCanary";
                    break;
                default:
                    throw new Exception($"Invalid settings value: {instance}");
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName);
            if(!Directory.Exists(path))
            {
                throw new Exception($"Couldn't find Discord folder path for {instance}");
            }

            foreach(var folder in Directory.GetDirectories(path))
            {
                string exePath = Path.Combine(folder, $"{folderName}.exe");
                if(new DirectoryInfo(folder).Name.StartsWith("app-") && File.Exists(exePath))
                {
                    Process.Start(exePath, $"--url -- \"discord://-/{url}\"");
                    return;
                }
            }

            throw new Exception("Couldn't find the Discord exe file");
        }

        public DateTime SnowflakeToTimestap(string snowflake)
        {
            var ms = Int64.Parse(snowflake) >> 22;
            var timestamp = ms + 1420070400000;

            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }

        private void DisplayMessage(DMessage message)
        {
            var msgControl = new MessageWPF(message, User);
            ((MessageListWPF)elementHost1.Child).AddToList(msgControl);
        }

        private List<DAnalyticsEvent> AllInvites = new List<DAnalyticsEvent>();
        private void ProcessAnalyticsLine(string line)
        {
            // Pro optimization
            if(!line.StartsWith("{\"event_type\":\"guild_joined") && !line.Contains("{\"event_type\":\"create_guild") && !line.Contains("{\"event_type\":\"accepted_instant_invite"))
            {
                return;
            }

            var eventData = Newtonsoft.Json.JsonConvert.DeserializeObject<DAnalyticsEvent>(line);

            switch (eventData.event_type)
            {
                case "guild_joined":
                case "guild_joined_pending":
                    var idx = AllJoinedGuilds.FindIndex(x => x.id == eventData.guild_id);
                    if (idx > -1)
                    {
                        var guild = AllJoinedGuilds[idx];
                        if (eventData.invite_code != null && !guild.invites.Contains(eventData.invite_code))
                        {
                            guild.invites.Add(eventData.invite_code);
                        }

                        // Get the earliest join date
                        var timestamp = DateTime.Parse(eventData.timestamp.Replace("\"", ""), null, System.Globalization.DateTimeStyles.RoundtripKind);
                        if(timestamp < guild.timestamp)
                        {
                            guild.timestamp = timestamp;
                        }
                    }
                    else
                    {
                        AllJoinedGuilds.Add(new DAnalyticsGuild
                        {
                            id = eventData.guild_id,
                            join_type = eventData.join_type,
                            join_method = eventData.join_method,
                            application_id = eventData.application_id,
                            location = eventData.location,
                            invites = (eventData.invite_code != null ? new List<string> { eventData.invite_code } : new List<string>()),
                            timestamp = DateTime.Parse(eventData.timestamp.Replace("\"", ""), null, System.Globalization.DateTimeStyles.RoundtripKind)
                        });
                    }
                    break;
                case "create_guild":
                    Debug.WriteLine(eventData.timestamp.Replace("\"", ""));
                    AllJoinedGuilds.Add(new DAnalyticsGuild
                    {
                        id = eventData.guild_id,
                        join_type = "created by you",
                        invites = new List<string>(),
                        timestamp = DateTime.Parse(eventData.timestamp.Replace("\"", ""), null, System.Globalization.DateTimeStyles.RoundtripKind)
                    });
                    break;
                case "accepted_instant_invite":
                    if(eventData.guild != null) AllInvites.Add(eventData);
                    break;
            }
        }

        private void loadFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loadFileBtn.Hide();
                progressBar1.Show();

                guildsBw.RunWorkerAsync();
                loadBw.RunWorkerAsync();
                loadTimer.Start();
            }
        }

        private string LoadStatusText = "";
        private int LoadProgress = 0;
        private void loadBw_DoWork(object sender, DoWorkEventArgs e)
        {
            var startTime = DateTime.Now;

            using (var file = File.OpenRead(openFileDialog1.FileName))
            using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
            {
                progressBar1.Invoke((MethodInvoker)delegate {
                    progressBar1.Maximum = zip.Entries.Count;
                });

                var userFile = zip.GetEntry("account/user.json");
                if (userFile == null)
                {
                    throw new Exception("Invalid data package: missing user.json");
                }

                PackageCreationTime = userFile.LastWriteTime.DateTime;

                using (var r = new StreamReader(userFile.Open()))
                {
                    var json = r.ReadToEnd();
                    User = Newtonsoft.Json.JsonConvert.DeserializeObject<DUser>(json);
                }

                using (var r = new StreamReader(zip.GetEntry("servers/index.json").Open()))
                {
                    var json = r.ReadToEnd();
                    CurrentGuilds = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
                }

                int i = 0;
                foreach (var entry in zip.Entries)
                {
                    i++;
                    LoadStatusText = $"Reading {entry.FullName}\n{i}/{zip.Entries.Count}";
                    LoadProgress = i;

                    if (Regex.IsMatch(entry.FullName, @"messages/c?\d+/messages\.csv", RegexOptions.None))
                    {
                        var match = Regex.Matches(entry.FullName, @"messages/(c?(\d+))/messages\.csv", RegexOptions.None)[0];
                        var channelId = match.Groups[2].Value;
                        var folderName = match.Groups[1].Value; // folder name might not start with "c" in older versions
                        using (var rJson = new StreamReader(zip.GetEntry($"messages/{folderName}/channel.json").Open()))
                        using (var rCsv = new StreamReader(entry.Open()))
                        {
                            var json = rJson.ReadToEnd();
                            var csv = rCsv.ReadToEnd();

                            var channel = Newtonsoft.Json.JsonConvert.DeserializeObject<DChannel>(json);
                            channel.LoadMessages(csv);

                            TotalMessages += channel.messages.Count;
                            Channels.Add(channel);
                        }
                    }
                }
            }

            AllAttachments = AllAttachments.OrderByDescending(o => Int64.Parse(o.message.id)).ToList();

            dmsLv.Invoke((MethodInvoker)delegate
            {
                var dmChannels = Channels.Where(x => x.IsDM()).OrderByDescending(o => Int64.Parse(o.id)).ToList();
                foreach (var dmChannel in dmChannels)
                {
                    string recipientId = dmChannel.GetOtherDMRecipient(User);
                    string recipientUsername = "";
                    var relationship = User.relationships.ToList().Find(x => x.id == recipientId);
                    if (relationship != null) recipientUsername = relationship.user.GetTag();

                    string[] values = { SnowflakeToTimestap(dmChannel.id).ToShortDateString(), dmChannel.id, recipientId, recipientUsername, dmChannel.messages.Count.ToString() };
                    var lvItem = new ListViewItem(values);
                    dmsLv.Items.Add(lvItem);
                }
            });

            loadingLb.Invoke((MethodInvoker)delegate {
                loadingLb.Text = $"Finished! Parsed {TotalMessages.ToString("N0", new NumberFormatInfo { NumberGroupSeparator = " " })} messages in {Math.Floor((DateTime.Now - startTime).TotalSeconds)}s\nPackage created at: {PackageCreationTime.ToShortDateString()}";
            });
        }
        private void loadTimer_Tick(object sender, EventArgs e)
        {
            loadingLb.Text = LoadStatusText;
            progressBar1.Value = LoadProgress;
        }

        private void loadBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadTimer.Stop();
            if (e.Error != null)
            {
                loadFileBtn.Show();
                progressBar1.Hide();

                MessageBox.Show($"An error occurred:\n\n{e.Error.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            progressBar1.Value = progressBar1.Maximum;
        }

        private List<DMessage> LastSearchResults;
        private int SearchResultsOffset = 0;
        private void LoadSearchResults()
        {
            ((MessageListWPF)elementHost1.Child).Clear();
            resultsCountLb.Text = $"{SearchResultsOffset + 1}-{Math.Min(SearchResultsOffset + MaxResults, LastSearchResults.Count)} of {LastSearchResults.Count}";
            for (int i = SearchResultsOffset; i < SearchResultsOffset + MaxResults; i++)
            {
                if(i >= LastSearchResults.Count) return;

                var msg = LastSearchResults[i];
                DisplayMessage(msg);
            }
        }
        private void searchBtn_Click(object sender, EventArgs e)
        {
            searchBtn.Enabled = false;
            searchTb.Enabled = false;
            searchOptionsBtn.Enabled = false;
            messagesPrevBtn.Enabled = false;
            messagesNextBtn.Enabled = false;
            messagesPanel.Hide();
            //messagesPanel.Controls.Clear();
            ((MessageListWPF)elementHost1.Child).Clear();

            searchBw.RunWorkerAsync();
            searchTimer.Start();
        }

        private void searchBw_DoWork(object sender, DoWorkEventArgs e)
        {
            SearchResultsOffset = 0;

            var searchText = searchTb.Text;
            int count = 0;

            LastSearchResults = new List<DMessage>();

            foreach (var channel in Channels)
            {
                // Filters
                if (channel.IsDM() && Properties.Settings.Default.SearchExcludeDMs) continue;
                if (channel.IsGroupDM() && Properties.Settings.Default.SearchExcludeGDMs) continue;
                if (!channel.IsDM() && !channel.IsGroupDM() && Properties.Settings.Default.SearchExcludeGuilds) continue;
                if (Properties.Settings.Default.SearchExcludeIDs != null)
                {
                    if (Properties.Settings.Default.SearchExcludeIDs.Contains(channel.id)) continue;
                    if (channel.guild != null && channel.guild.id != null && Properties.Settings.Default.SearchExcludeIDs.Contains(channel.guild.id)) continue;
                }
                if(Properties.Settings.Default.SearchWhitelistIDs != null && Properties.Settings.Default.SearchWhitelistIDs.Count > 0)
                {
                    if (!Properties.Settings.Default.SearchWhitelistIDs.Contains(channel.id) && !(channel.guild != null && channel.guild.id != null && Properties.Settings.Default.SearchWhitelistIDs.Contains(channel.guild.id))) continue;
                }

                // Search modes
                if (Properties.Settings.Default.SearchMode == "exact")
                {
                    count += SearchExact(searchText, channel);
                }
                else if (Properties.Settings.Default.SearchMode == "words")
                {
                    count += SearchWords(searchText, channel);
                }
                else if (Properties.Settings.Default.SearchMode == "regex")
                {
                    count += SearchRegex(searchText, channel);
                }
            }

            LastSearchResults = LastSearchResults.OrderByDescending(o => Int64.Parse(o.id)).ToList();
            foreach(var message in LastSearchResults)
            {
                if(Properties.Settings.Default.DeletedMessageIDs.Contains(message.id))
                {
                    message.deleted = true;
                }
            }
        }

        private void searchTimer_Tick(object sender, EventArgs e)
        {
            if (LastSearchResults.Count > 0)
            {
                if (LastSearchResults.Count >= MaxResults)
                {
                    resultsCountLb.Text = $"{SearchResultsOffset + 1}-{SearchResultsOffset + MaxResults} of {LastSearchResults.Count}";
                }
                else
                {
                    resultsCountLb.Text = $"{SearchResultsOffset + 1}-{LastSearchResults.Count} of {LastSearchResults.Count}";
                }
            }
        }

        private void searchBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            searchTimer.Stop();

            if (LastSearchResults.Count > 0)
            {
                LoadSearchResults();

                if (LastSearchResults.Count >= MaxResults)
                {
                    resultsCountLb.Text = $"{SearchResultsOffset + 1}-{SearchResultsOffset + MaxResults} of {LastSearchResults.Count}";
                }
                else
                {
                    resultsCountLb.Text = $"{SearchResultsOffset + 1}-{LastSearchResults.Count} of {LastSearchResults.Count}";
                }
            } else
            {
                resultsCountLb.Text = "No results";
            }

            messagesPanel.Show();
            searchBtn.Enabled = true;
            searchTb.Enabled = true;
            searchOptionsBtn.Enabled = true;
            messagesPrevBtn.Enabled = true;
            messagesNextBtn.Enabled = true;
        }

        private int SearchExact(string searchText, DChannel channel)
        {
            int count = 0;

            foreach (var msg in channel.messages)
            {

                if (msg.content.Contains(searchText))
                {
                    LastSearchResults.Add(msg);
                    count++;
                }
            }

            return count;
        }

        private int SearchWords(string searchText, DChannel channel)
        {
            int count = 0;

            foreach (var msg in channel.messages)
            {
                bool isMatch = true;
                foreach (var word in searchText.Split(' '))
                {
                    if (!Regex.IsMatch(msg.content, $@"(^|\s+){word}($|\s+)", RegexOptions.None))
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    LastSearchResults.Add(msg);
                    count++;
                }
            }

            return count;
        }

        private int SearchRegex(string searchText, DChannel channel)
        {
            int count = 0;

            foreach (var msg in channel.messages)
            {

                if (Regex.IsMatch(msg.content, searchText, RegexOptions.None))
                {
                    LastSearchResults.Add(msg);
                    count++;
                }
            }

            return count;
        }

        private void messagesPrevBtn_Click(object sender, EventArgs e)
        {
            SearchResultsOffset -= MaxResults;
            LoadSearchResults();
        }

        private void messagesNextBtn_Click(object sender, EventArgs e)
        {
            SearchResultsOffset += MaxResults;
            LoadSearchResults();
        }

        private int imagesOffset = 0;
        private int imagesPerPage = 36;
        private int imagesPerRow = 9;
        private int imageSquareSize = 200;
        private void LoadImages()
        {
            imagesNextBtn.Enabled = false;
            imagesPrevBtn.Enabled = false;

            if (imagesOffset < 0) imagesOffset = 0;
            if (imagesOffset >= AllAttachments.Count || imagesOffset + imagesPerPage >= AllAttachments.Count) imagesOffset = AllAttachments.Count - imagesPerPage;

            imagesPanel.Controls.Clear();
            imagesCountLb.Text = $"{imagesOffset + 1}-{imagesOffset + imagesPerPage} of {AllAttachments.Count}";

            for (int i = 0; i < imagesPerPage; i++)
            {
                var loc = new Point(3, 3);
                if (imagesPanel.Controls.Count > 0)
                {
                    var last = (Attachment)imagesPanel.Controls[imagesPanel.Controls.Count - 1];
                    loc = new Point(imagesPanel.Controls.Count % imagesPerRow == 0 ? 3 : last.Location.X + last.Size.Width + 6, imagesPanel.Controls.Count % imagesPerRow == 0 ? last.Location.Y + imageSquareSize + 44 : last.Location.Y);
                }

                var attachment = AllAttachments[imagesOffset + i];

                var pb = new Attachment(attachment)
                {
                    Size = new Size(imageSquareSize, imageSquareSize),
                    Location = loc,
                    Parent = imagesPanel,
                };

                ThreadPool.QueueUserWorkItem(state => pb.LoadImage());

                Application.DoEvents();
            }

            imagesNextBtn.Enabled = true;
            imagesPrevBtn.Enabled = true;
        }
        private void imagesNextBtn_Click(object sender, EventArgs e)
        {
            if(imagesPanel.Controls.Count > 0)
            {
                imagesOffset += imagesPerPage;
            }
            LoadImages();
        }

        private void imagesPrevBtn_Click(object sender, EventArgs e)
        {
            if (imagesPanel.Controls.Count > 0)
            {
                imagesOffset -= imagesPerPage;
            }
            LoadImages();
        }

        private void imagesCountLb_DoubleClick(object sender, EventArgs e)
        {
            var offset = Interaction.InputBox("Enter the offset number", "Prompt");
            try
            {
                imagesOffset = Int32.Parse(offset);
                LoadImages();
            }
            catch (Exception) { }
        }

        private void discordInstanceSettingsChange(object sender, EventArgs e)
        {
            if(defaultRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "default";
            } else if(stableRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "stable";
            } else if(ptbRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "ptb";
            } else if(canaryRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "canary";
            } else if(webStableRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "web_stable";
            }
            else if (webPTBRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "web_ptb";
            }
            else if (webCanaryRb.Checked)
            {
                Properties.Settings.Default.UseDiscordInstance = "web_canary";
            }

            Properties.Settings.Default.Save();
        }

        private void guildsBw_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var file = File.OpenRead(openFileDialog1.FileName))
            using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    if (Regex.IsMatch(entry.FullName, @"activity/reporting/events.+\.json", RegexOptions.None))
                    {
                        using (var data = new StreamReader(entry.Open()))
                        {
                            int lineNum = 0;
                            while (!data.EndOfStream)
                            {
                                lineNum++;

                                var line = data.ReadLine();
                                //ThreadPool.QueueUserWorkItem(state => ProcessAnalyticsLine(line));
                                ProcessAnalyticsLine(line);
                            }
                        }
                    }
                }
            }
        }

        private void guildsBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var eventData in AllInvites)
            {
                var guild = AllJoinedGuilds.Find(x => x.id == eventData.guild);
                if (guild == null)
                {
                    AllJoinedGuilds.Add(new DAnalyticsGuild
                    {
                        id = eventData.guild,
                        join_type = "invite",
                        invites = new List<string> { eventData.invite },
                        timestamp = DateTime.Parse(eventData.timestamp.Replace("\"", ""), null, System.Globalization.DateTimeStyles.RoundtripKind)
                    });
                }
                else if (!guild.invites.Contains(eventData.invite))
                {
                    guild.invites.Add(eventData.invite);
                }
            }

            AllJoinedGuilds = AllJoinedGuilds.OrderByDescending(o => o.timestamp.Ticks).ToList();

            serversLv.Items.Clear();
            foreach (var guild in AllJoinedGuilds)
            {
                string guildName = "";
                if(CurrentGuilds[guild.id] != null)
                {
                    guildName = CurrentGuilds[guild.id];
                }

                string[] values = { guild.timestamp.ToShortDateString(), guild.id, guildName, guild.join_type, guild.location, String.Join(", ", guild.invites.ToArray()) };
                var lvItem = new ListViewItem(values);
                serversLv.Items.Add(lvItem);
            }
        }

        private int MassDeleteIdx = 0;
        private void massDeleteBtn_Click(object sender, EventArgs e)
        {
            if(massDeleteTimer.Enabled == true)
            {
                massDeleteTimer.Stop();
                massDeleteBtn.Text = "Mass Delete";
                searchTb.Enabled = true;
                searchBtn.Enabled = true;
                return;
            }

            if(LastSearchResults == null || LastSearchResults.Count == 0)
            {
                MessageBox.Show("You need to search for something first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var prompt = new MassDeletePrompt();
            prompt.ShowDialog();
            if(prompt.DialogSuccess)
            {
                MassDeleteIdx = 0;
                massDeleteTimer.Interval = prompt.GetDelay();
                AccountToken = prompt.GetToken();

                searchTb.Enabled = false;
                searchBtn.Enabled = false;
                massDeleteBtn.Text = "Click to stop";

                DHeaders.Init();
                massDeleteTimer.Start();
            }
        }

        private void massDeleteTimer_Tick(object sender, EventArgs e)
        {
            massDeleteTimer.Stop(); // Stop and restart the timer every time to prevent overlaps

            DMessage msg;
            while(true)
            {
                if (MassDeleteIdx >= LastSearchResults.Count)
                {
                    massDeleteBtn.Text = "Mass Delete";
                    searchTb.Enabled = true;
                    searchBtn.Enabled = true;

                    MessageBox.Show("Mass Delete Finished!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                msg = LastSearchResults[MassDeleteIdx++];
                if (!msg.deleted) break;
            }

            try
            {
                // TODO: request here
                var res = DRequest.Request("DELETE", $"https://discord.com/api/v9/channels/{msg.channel.id}/messages/{msg.id}", new Dictionary<string, string>
                {
                    {"Authorization", AccountToken}
                });
                
                switch(res.response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.NoContent:
                        msg.deleted = true;
                        ((MessageListWPF)elementHost1.Child).RemoveMessage(msg.id);

                        Properties.Settings.Default.DeletedMessageIDs.Add(msg.id);
                        Properties.Settings.Default.Save();
                        break;
                    case (HttpStatusCode)429:
                        MassDeleteIdx--;
                        System.Threading.Thread.Sleep((int)Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res.body).retry_after * 1000);
                        break;
                    case HttpStatusCode.Forbidden:
                        break;
                    default:
                        MessageBox.Show($"Request error: {res.response.StatusCode} {res.body}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            } catch(Exception ex)
            {
                MessageBox.Show($"Request error: {ex.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            massDeleteTimer.Start();
        }

        private void copyIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(serversLv.SelectedItems[0].SubItems[1].Text);
        }

        private void copyInvitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(serversLv.SelectedItems[0].SubItems[5].Text);
        }

        private void searchOptionsBtn_Click(object sender, EventArgs e)
        {
            var prompt = new SearchOptionsPrompt();
            prompt.ShowDialog();
        }

        private void copyUserIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dmsLv.SelectedItems[0].SubItems[2].Text);
        }

        private void copyChannelIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dmsLv.SelectedItems[0].SubItems[1].Text);
        }

        private void viewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Main.LaunchDiscordProtocol($"users/{dmsLv.SelectedItems[0].SubItems[2].Text}");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static bool ValidateToken(string token)
        {
            var parts = token.Split('.');

            if (parts.Length != 3) return false;

            var userIdPart = parts[0];
            try
            {
                var userId = Encoding.UTF8.GetString(Convert.FromBase64String(userIdPart));
                return userId == User.id;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
