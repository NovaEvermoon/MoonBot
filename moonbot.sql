-- phpMyAdmin SQL Dump
-- version 4.8.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 08, 2019 at 01:39 PM
-- Server version: 10.1.32-MariaDB
-- PHP Version: 7.2.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `moonbot`
--

-- --------------------------------------------------------

--
-- Table structure for table `burps`
--

CREATE TABLE `burps` (
  `burps_total` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `burps`
--

INSERT INTO `burps` (`burps_total`) VALUES
(80);

-- --------------------------------------------------------

--
-- Table structure for table `command`
--

CREATE TABLE `command` (
  `command_id` int(11) NOT NULL,
  `command_keyword` varchar(150) COLLATE utf8_bin NOT NULL,
  `command_message` varchar(1000) COLLATE utf8_bin NOT NULL,
  `command_userLevel` varchar(150) COLLATE utf8_bin NOT NULL,
  `command_cooldown` int(99) NOT NULL,
  `command_status` tinyint(1) NOT NULL,
  `command_timer` int(11) NOT NULL,
  `command_description` varchar(500) COLLATE utf8_bin NOT NULL,
  `command_type` varchar(500) COLLATE utf8_bin NOT NULL,
  `command_request` varchar(5000) COLLATE utf8_bin NOT NULL,
  `command_parameters` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `command`
--

INSERT INTO `command` (`command_id`, `command_keyword`, `command_message`, `command_userLevel`, `command_cooldown`, `command_status`, `command_timer`, `command_description`, `command_type`, `command_request`, `command_parameters`) VALUES
(1, 'nova', 'Follow me on social media! ♡ https://linktr.ee/nova.evermoon', 'everyone', 10000, 1, 0, 'Display Nova\'s social media', 'regular', '', 0),
(2, 'humble', 'Do you like video games? Do you like saving money? Would you like to get games cheaper while supporting me and also charity? Look no further, humble bundle is for you ! Type !monthly for the link to the monthly bundle or !store for the link to the store ', 'everyone', 15000, 1, 0, 'Gives the information and links about humble bundle ', 'regular', '', 0),
(3, 'follow', 'Are you enjoying your time here? Don\'t forget to smash that follow button so you can come and hang out next time I\'m live ! ♡', 'everyone', 0, 1, 2820000, 'Following message timed command', 'timed', '', 0),
(4, 'store', 'Humble bundle store : https://www.humblebundle.com/store?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get the humble store partner link', 'regular', '', 0),
(5, 'monthly', 'Humble bundle monthly : https://www.humblebundle.com/monthly?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get humble monthly link', 'regular', '', 0),
(6, 'crystals', 'Crystals are the channel\'s currency, you earn them while watching the stream and by following, hosting, subscribing etc. You can use these to buy neat rewards ! Leaderboard : https://streamlabs.com/novaevermoon#/loyalty', 'everyone', 10000, 1, 0, 'Get crystal infos', 'regular', '', 0),
(7, 'lurk', ' is going into lurk mode ! Thank you ♡', 'everyone', 10000, 1, 0, 'sending message for lurk ', 'regular', '', 0),
(8, 'discord', 'Wanna be part of the cool kids and hang out after stream? You can get access to my discord either by subbing or with crystals (!crystals for more info :3 )', 'everyone', 10000, 1, 0, 'get discord info', 'regular', '', 0),
(9, 'commands', 'List of commands you can use in the chat : ', 'everyone', 10000, 1, 0, 'Display list of usable commands', 'regular', '', 0),
(10, 'prime', 'Did you know that you can get a free subscription to the channel with twitch prime ! Follow that link to learn more about it ! https://twitch.amazon.com/prime ', 'everyone', 0, 1, 4440000, 'Display information about twitch prime', 'timed', '', 0),
(11, 'burp', 'The burp count has been updated ! ', 'moderator', 1000, 1, 0, 'Add burps to the burp total', 'request', 'UPDATE burps SET burps_total = burps_total+1', 0),
(12, 'burps', 'Nova has burped @ times so far !', 'everyone', 10000, 1, 0, 'get the total number of burps', 'request', 'SELECT * FROM burps', 0),
(13, 'charity', 'From December 12-27, you can cheer with #charity and Twitch will donate $.20 for every 100 Bits to DirectRelief! You\'ll also be able to snag a cute  shiny new charity badge, so make dem bitties rain ! ', 'everyone', 15000, 1, 0, 'charity explanation', 'regular', '', 0),
(15, 'birthday', 'Nova\'s birthday was on the 8th of December. For this occasion, she is doing a pc fund push for the ENTIRE month of december ! Sepcial rewards and goals can be unlocked with donations & bits ! Type !goals or !rewards to learn more about it', 'everyone', 0, 0, 1800000, 'Birthday event explanation ', 'timed', '0', 0),
(16, 'rewards', '1€/100 bits : Name in hall of fame\r\n2€/200 bits : Custom command on stream\r\n5€/500 bits : Name on Balloon\r\n10€/1000 bits : Postal Card\r\n20€/2000 bits : Pokemon Pearler Bead\r\n50€/5000 bits : All rewards', 'everyone', 15000, 0, 0, 'birthday rewards', 'regular', '', 0),
(17, 'goals', '200€ : Bestfriend does my makeup (completed)\r\n400€ : Vegan cheescake cooking stream with Chaton\r\n600€ : Helium Karaoke\r\n800€ : Spyro giveaway (PS4)\r\n1000€ : PC Building Stream\r\n1200€(stretch goal) : Cosplay Stream\r\n', 'everyone', 15000, 1, 0, 'Birthday goals ', 'regular', '', 0),
(18, 'duo', 'Today, Nova is duo streaming Human Fall Flat with Dudoiselle ! Head to  https://multistre.am/dudoiselle/novaevermoon/layout4/ to watch both the strims !', 'everyone', 10000, 0, 0, 'duo stream', 'regular', '', 0),
(19, 'addicted', 'toKappa', 'everyone', 10000, 1, 0, 'birthday command for addictedtokappa', 'regular', '', 0),
(20, 'giveaway', 'Ho ho ho ! To thank you all for making this year amazing, I\'m gonna be giving away a copy Spyro Reignited for PS4 so be sure to hang out today :3 ', 'everyone', 15000, 0, 0, 'Christmas Giveaway ', 'regular', '', 0),
(21, 'menu', 'For today\'s cooking stream, we will me making : Garlic Puff pastry, Garlic mushrooms, Seitan roast with mushroom sauce (yeah we really like mushrooms :B), potatoe gratin, Pecan mapple puff pastry and sugar cookies !', 'everyone', 15000, 0, 0, 'Christmas menu', 'regular', '', 0),
(22, 'HinaNumbaOne', 'My gold medal, don\'t test me Kappa', 'everyone', 15000, 1, 0, 'Hina\'s command', 'regular', '', 0),
(23, 'followage', 'getFollowage', 'everyone', 15000, 1, 0, 'getFollowage', 'api', '', 1),
(24, 'stretch', 'It\'s time to take a short break, stretch dese legs and grab a drink ! ', 'everyone', 0, 1, 3420000, 'Stretch Command', 'timed', '', 0),
(25, 'pc', 'Nova is currently streaming on a broken ass laptop and is pushing for a new pc ! We\'re almost there ! Thank you everyone for the support :3 ', 'everyone', 15000, 1, 0, 'Pc push description', 'regular', '', 0),
(26, 'bsg', 'Please refrain from backsteating ! Especially with games with a great storyline/mechanics like Dark Souls. Nova asks a lot of questions but these are mainly rethorical and talking out loud, if she needs an answer she will directly ask the chat ! ', 'everyone', 10000, 1, 0, 'BackseatGaming ', 'regular', '', 0),
(27, 'title', 'getChannelTitle', 'everyone', 15000, 1, 0, 'get the stream\'s current title', 'api', '', 0),
(28, 'game', 'getChannelGame', 'everyone', 15000, 1, 0, 'get channel\'s current game', 'api', '', 0);

-- --------------------------------------------------------

--
-- Table structure for table `follower`
--

CREATE TABLE `follower` (
  `follower_id` int(11) NOT NULL,
  `follower_createdAt` datetime NOT NULL,
  `follower_user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `user_id` int(11) NOT NULL,
  `user_displayName` varchar(200) COLLATE utf8_bin NOT NULL,
  `user_name` varchar(200) COLLATE utf8_bin NOT NULL,
  `user_twitchId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `command`
--
ALTER TABLE `command`
  ADD PRIMARY KEY (`command_id`);

--
-- Indexes for table `follower`
--
ALTER TABLE `follower`
  ADD PRIMARY KEY (`follower_id`),
  ADD KEY `follower_user_id` (`follower_user_id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `command`
--
ALTER TABLE `command`
  MODIFY `command_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `follower`
--
ALTER TABLE `follower`
  MODIFY `follower_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `follower`
--
ALTER TABLE `follower`
  ADD CONSTRAINT `fk_user` FOREIGN KEY (`follower_user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
