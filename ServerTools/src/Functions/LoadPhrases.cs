﻿using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ServerTools
{
    public class Phrases
    {
        public static SortedDictionary<int, string> _Phrases = new SortedDictionary<int, string>();
        private static string _file = "ServerToolsPhrases.xml";
        private static string _filepath = string.Format("{0}/{1}", Config._configpath, _file);
        private static FileSystemWatcher _fileWatcher = new FileSystemWatcher(Config._configpath, _file);

        public static void Init()
        {
            if (!Utils.FileExists(_filepath))
            {
                UpdatePhrases();
            }
            LoadPhrases();
            InitFileWatcher();
        }

        private static void LoadPhrases()
        {
            if (!Utils.FileExists(_filepath))
            {
                UpdatePhrases();
            }
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(_filepath);
            }
            catch (XmlException e)
            {
                Log.Error(string.Format("[SERVERTOOLS] Failed loading {0}: {1}", _file, e.Message));
                return;
            }
            XmlNode _PhrasesXml = xmlDoc.DocumentElement;
            _Phrases.Clear();
            foreach (XmlNode childNode in _PhrasesXml.ChildNodes)
            {
                if (childNode.Name == "Phrases")
                {
                    foreach (XmlNode subChild in childNode.ChildNodes)
                    {
                        if (subChild.NodeType == XmlNodeType.Comment)
                        {
                            continue;
                        }
                        if (subChild.NodeType != XmlNodeType.Element)
                        {
                            Log.Warning(string.Format("[SERVERTOOLS] Unexpected XML node found in 'Phrases' section: {0}", subChild.OuterXml));
                            continue;
                        }
                        XmlElement _line = (XmlElement)subChild;
                        if (!_line.HasAttribute("id"))
                        {
                            Log.Warning(string.Format("[SERVERTOOLS] Ignoring Phrase entry because of missing 'id' attribute: {0}", subChild.OuterXml));
                            continue;
                        }
                        if (!_line.HasAttribute("Phrase"))
                        {
                            Log.Warning(string.Format("[SERVERTOOLS] Ignoring Phrase entry because of missing 'Phrase' attribute: {0}", subChild.OuterXml));
                            continue;
                        }
                        int _id;
                        if (!int.TryParse(_line.GetAttribute("id"), out _id))
                        {
                            Log.Warning(string.Format("[SERVERTOOLS] Ignoring Phrase entry because of invalid (non-numeric) value for 'id' attribute: {0}", subChild.OuterXml));
                            continue;
                        }
                        if (!_Phrases.ContainsKey(_id))
                        {
                            _Phrases.Add(_id, _line.GetAttribute("Phrase"));
                        }
                    }
                }
            }
        }

        public static void UpdatePhrases()
        {
            _fileWatcher.EnableRaisingEvents = false;
            using (StreamWriter sw = new StreamWriter(_filepath))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.WriteLine("<ServerTools>");
                sw.WriteLine("    <Phrases>");
                if (_Phrases.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _Phrases)
                    {
                        sw.WriteLine(string.Format("        <Phrase id=\"{0}\" Phrase=\"{1}\" />", kvp.Key, kvp.Value));
                    }
                }
                else
                {
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- *************** High Ping Kicker Phrases *************** -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"1\" Phrase=\"Auto Kicking {PlayerName} for high ping. ({PlayerPing}) Maxping is {MaxPing}.\" />");
                    sw.WriteLine("        <Phrase id=\"2\" Phrase=\"Auto Kicked: Ping To High. ({PlayerPing}) Max Ping is {MaxPing}.\" />");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- ***************** Invalid Item Phrases ***************** -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"3\" Phrase=\"{PlayerName} you have a invalid item stack: {ItemName} {ItemCount}. Max per stack: {MaxPerStack}.\" />");
                    sw.WriteLine("        <Phrase id=\"4\" Phrase=\"Cheat Detected: Auto banned {PlayerName} for having a invalid item: {ItemName}.\" />");
                    sw.WriteLine("        <Phrase id=\"5\" Phrase=\"Cheat Detected: Auto kicked {PlayerName} for having a invalid item: {ItemName}.\" />");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- ************************* Gimme ************************ -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"6\" Phrase=\"{PlayerName} you can only use Gimme once every {DelayBetweenUses} minutes. Time remaining: {TimeRemaining} minutes.\" />");
                    sw.WriteLine("        <Phrase id=\"7\" Phrase=\"{PlayerName} has received {ItemCount} {ItemName}.\" />");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- ************************ Killme ************************ -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"8\" Phrase=\"{PlayerName} you can only use /killme once every {DelayBetweenUses} minutes. Time remaining: {TimeRemaining} minutes.\" />");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- *********************** SetHome ************************ -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"9\" Phrase=\"{PlayerName} you already have a home set.\" />");
                    sw.WriteLine("        <Phrase id=\"10\" Phrase=\"{PlayerName} your home has been saved.\" />");
                    sw.WriteLine("        <Phrase id=\"11\" Phrase=\"{PlayerName} you do not have a home saved.\" />");
                    sw.WriteLine("        <Phrase id=\"12\" Phrase=\"{PlayerName} your home has been removed.\" />");
                    sw.WriteLine("        <Phrase id=\"13\" Phrase=\"{PlayerName} you can only use /home once every {DelayBetweenUses} minutes. Time remaining: {TimeRemaining} minutes.\" />");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <!-- ****************** Clan Tag Protection ***************** -->");
                    sw.WriteLine("        <!-- ******************************************************** -->");
                    sw.WriteLine("        <Phrase id=\"14\" Phrase=\"You do not belong to the clan {ClanName}. Please remove the clan tag and rejoin.\" />");
                    sw.WriteLine("        <Phrase id=\"15\" Phrase=\"{PlayerName} you have already created the clan {ClanName}.\" />");
                    sw.WriteLine("        <Phrase id=\"16\" Phrase=\"The clan {ClanName} already exist.\" />");
                    sw.WriteLine("        <Phrase id=\"17\" Phrase=\"You are currently a member of the clan {ClanName}.\" />");
                    sw.WriteLine("        <Phrase id=\"18\" Phrase=\"The clan {ClanName} has been added.\" />");
                    sw.WriteLine("        <Phrase id=\"19\" Phrase=\"Something went wrong, could not add the clan {ClanName}.\" />");
                    sw.WriteLine("        <Phrase id=\"20\" Phrase=\"You are not the owner of any clans.\" />");
                    sw.WriteLine("        <Phrase id=\"21\" Phrase=\"The clan {ClanName} has been removed.\" />");
                    sw.WriteLine("        <Phrase id=\"22\" Phrase=\"You do not have permissions to use this command.\" />");
                    sw.WriteLine("        <Phrase id=\"23\" Phrase=\"Player name not found.\" />");
                    sw.WriteLine("        <Phrase id=\"24\" Phrase=\"{PlayerName} is already a member of a clan.\" />");
                    sw.WriteLine("        <Phrase id=\"25\" Phrase=\"{PlayerName} already has pending clan invites.\" />");
                    sw.WriteLine("        <Phrase id=\"26\" Phrase=\"You have been invited to join the clan {ClanName}. Type /clanaccept to join or /clandecline to decline the offer.\" />");
                    sw.WriteLine("        <Phrase id=\"27\" Phrase=\"Invite has been sent.\" />");
                    sw.WriteLine("        <Phrase id=\"28\" Phrase=\"You have not been invited to any clans.\" />");
                    sw.WriteLine("        <Phrase id=\"29\" Phrase=\"Clan could not be found.\" />");
                    sw.WriteLine("        <Phrase id=\"30\" Phrase=\"{PlayerName} has joined the clan.\" />");
                    sw.WriteLine("        <Phrase id=\"31\" Phrase=\"You have declined the invite to the clan.\" />");
                    sw.WriteLine("        <Phrase id=\"32\" Phrase=\"{PlayerName} is not a member of your clan.\" />");
                    sw.WriteLine("        <Phrase id=\"33\" Phrase=\"Only the clan owner can remove officers.\" />");
                    sw.WriteLine("        <Phrase id=\"34\" Phrase=\"Clan owners can not be removed.\" />");
                    sw.WriteLine("        <Phrase id=\"35\" Phrase=\"You have removed {PlayerName} from clan {ClanName}.\" />");
                    sw.WriteLine("        <Phrase id=\"36\" Phrase=\"You have been removed from clan {ClanName}.\" />");
                    sw.WriteLine("        <Phrase id=\"37\" Phrase=\"{PlayerName} is already a officer.\" />");
                    sw.WriteLine("        <Phrase id=\"38\" Phrase=\"{PlayerName} has been promoted to an officer.\" />");
                    sw.WriteLine("        <Phrase id=\"39\" Phrase=\"{PlayerName} is not an officer.\" />");
                    sw.WriteLine("        <Phrase id=\"40\" Phrase=\"{PlayerName} has been demoted.\" />");
                }
                sw.WriteLine("    </Phrases>");
                sw.WriteLine("</ServerTools>");
                sw.Flush();
                sw.Close();
            }
            _fileWatcher.EnableRaisingEvents = true;
        }

        private static void InitFileWatcher()
        {
            _fileWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
            _fileWatcher.Created += new FileSystemEventHandler(OnFileChanged);
            _fileWatcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            _fileWatcher.EnableRaisingEvents = true;
        }

        private static void OnFileChanged(object source, FileSystemEventArgs e)
        {
            if (!Utils.FileExists(_filepath))
            {
                UpdatePhrases();
            }
            LoadPhrases();
        }
    }
}