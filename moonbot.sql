-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Dec 27, 2018 at 07:44 PM
-- Server version: 5.7.23
-- PHP Version: 7.2.10

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

DROP TABLE IF EXISTS `burps`;
CREATE TABLE IF NOT EXISTS `burps` (
  `burps_total` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `burps`
--

INSERT INTO `burps` (`burps_total`) VALUES
(8);

-- --------------------------------------------------------

--
-- Table structure for table `command`
--

DROP TABLE IF EXISTS `command`;
CREATE TABLE IF NOT EXISTS `command` (
  `command_id` int(11) NOT NULL AUTO_INCREMENT,
  `command_keyword` varchar(150) COLLATE utf8_bin NOT NULL,
  `command_message` varchar(1000) COLLATE utf8_bin NOT NULL,
  `command_userLevel` varchar(150) COLLATE utf8_bin NOT NULL,
  `command_cooldown` int(99) NOT NULL,
  `command_status` tinyint(1) NOT NULL,
  `command_timer` int(11) NOT NULL,
  `command_description` varchar(500) COLLATE utf8_bin NOT NULL,
  `command_type` varchar(500) COLLATE utf8_bin NOT NULL,
  `command_request` varchar(5000) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`command_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `command`
--

INSERT INTO `command` (`command_id`, `command_keyword`, `command_message`, `command_userLevel`, `command_cooldown`, `command_status`, `command_timer`, `command_description`, `command_type`, `command_request`) VALUES
(1, 'nova', 'Follow me on social media! ♡ https://linktr.ee/nova.evermoon', 'everyone', 10000, 1, 0, 'Display Nova\'s social media', 'regular', ''),
(2, 'humble', 'Do you like video games? Do you like saving money? Would you like to get games cheaper while supporting me and also charity? Look no further, humble bundle is for you ! Type !monthly for the link to the monthly bundle or !store for the link to the store ', 'everyone', 15000, 1, 0, 'Gives the information and links about humble bundle ', 'regular', ''),
(3, 'follow', 'Are you enjoying your time here? Don\'t forget to smash that follow button so you can come and hang out next time I\'m live ! ♡', 'everyone', 0, 1, 2820000, 'Following message timed command', 'timed', ''),
(4, 'store', 'Humble bundle store : https://www.humblebundle.com/store?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get the humble store partner link', 'regular', ''),
(5, 'monthly', 'Humble bundle monthly : https://www.humblebundle.com/monthly?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get humble monthly link', 'regular', ''),
(6, 'crystals', 'Crystals are the channel\'s currency, you earn them while watching the stream and by following, hosting, subscribing etc. You can use these to buy neat rewards ! Leaderboard : https://streamlabs.com/novaevermoon#/loyalty', 'everyone', 10000, 1, 0, 'Get crystal infos', 'regular', ''),
(7, 'lurk', 'is going into lurk mode ! Thank you ♡', 'everyone', 10000, 1, 0, 'sending message for lurk ', 'regular', ''),
(8, 'discord', 'Wanna be part of the cool kids and hang out after stream? You can get access to my discord either by subbing or with crystals (!crystals for more info :3 )', 'everyone', 10000, 1, 0, 'get discord info', 'regular', ''),
(9, 'commands', 'List of commands you can use in the chat : ', 'everyone', 10000, 1, 0, 'Display list of usable commands', 'regular', ''),
(10, 'prime', 'Did you know that you can get a free subscription to the channel with twitch prime ! Follow that link to learn more about it ! https://twitch.amazon.com/prime ', 'everyone', 0, 1, 4440000, 'Display information about twitch prime', 'timed', ''),
(11, 'burp', 'The burp count has been updated ! ', 'moderator', 1000, 1, 0, 'Add burps to the burp total', 'request', 'UPDATE burps SET burps_total = burps_total+1'),
(12, 'burps', 'Nova has burped @ times so far !', 'everyone', 10000, 1, 0, 'get the total number of burps', 'request', 'SELECT * FROM burps'),
(13, 'charity', 'From December 12-27, you can cheer with #charity and Twitch will donate $.20 for every 100 Bits to DirectRelief! You\'ll also be able to snag a cute  shiny new charity badge, so make dem bitties rain ! ', 'everyone', 15000, 1, 0, 'charity explanation', 'regular', ''),
(15, 'birthday', 'Nova\'s birthday was on the 8th of December. For this occasion, she is doing a pc fund push for the ENTIRE month of december ! Sepcial rewards and goals can be unlocked with donations & bits ! Type !goals or !rewards to learn more about it', 'everyone', 0, 1, 1800000, 'Birthday event explanation ', 'timed', '0'),
(16, 'rewards', '1€/100 bits : Name in hall of fame\r\n2€/200 bits : Custom command on stream\r\n5€/500 bits : Name on Balloon\r\n10€/1000 bits : Postal Card\r\n20€/2000 bits : Pokemon Pearler Bead\r\n50€/5000 bits : All rewards', 'everyone', 15000, 1, 0, 'birthday rewards', 'regular', ''),
(17, 'goals', '200€ : Bestfriend does my makeup (completed)\r\n400€ : Vegan cheescake cooking stream with Chaton\r\n600€ : Helium Karaoke\r\n800€ : Spyro giveaway (PS4)\r\n1000€ : PC Building Stream\r\n1200€(stretch goal) : Cosplay Stream\r\n', 'everyone', 15000, 1, 0, 'Birthday goals ', 'regular', ''),
(18, 'duo', 'Today, Nova is duo streaming Human Fall Flat with Dudoiselle ! Head to  https://multistre.am/dudoiselle/novaevermoon/layout4/ to watch both the strims !', 'everyone', 10000, 0, 0, 'duo stream', 'regular', ''),
(19, 'addicted', 'toKappa', 'everyone', 10000, 1, 0, 'birthday command for addictedtokappa', 'regular', ''),
(20, 'giveaway', 'Ho ho ho ! To thank you all for making this year amazing, I\'m gonna be giving away a copy Spyro Reignited for PS4 so be sure to hang out today :3 ', 'everyone', 15000, 1, 0, 'Christmas Giveaway ', 'regular', ''),
(21, 'menu', 'For today\'s cooking stream, we will me making : Garlic Puff pastry, Garlic mushrooms, Seitan roast with mushroom sauce (yeah we really like mushrooms :B), potatoe gratin, Pecan mapple puff pastry and sugar cookies !', 'everyone', 15000, 1, 0, 'Christmas menu', 'regular', '');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
