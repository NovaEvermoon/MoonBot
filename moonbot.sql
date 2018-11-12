-- phpMyAdmin SQL Dump
-- version 4.8.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 12, 2018 at 05:31 PM
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
  `command_description` varchar(500) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `command`
--

INSERT INTO `command` (`command_id`, `command_keyword`, `command_message`, `command_userLevel`, `command_cooldown`, `command_status`, `command_timer`, `command_description`) VALUES
(1, 'nova', 'Follow me on social media! ♡ https://linktr.ee/nova.evermoon', 'everyone', 10000, 1, 0, 'Display Nova\'s social media'),
(2, 'humble', 'Do you like video games? Do you like saving money? Would you like to get games cheaper while supporting me and also charity? Look no further, humble bundle is for you ! Type !monthly for the link to the monthly bundle or !store for the link to the store ', 'everyone', 0, 1, 1200000, 'Gives the information and links about humble bundle '),
(3, 'follow', 'Are you enjoying your time here? Don\'t forget to smash that follow button so you can come and hang out next time I\'m live ! ♡', 'everyone', 0, 1, 900000, 'Following message timed command'),
(4, 'store', 'Humble bundle store : https://www.humblebundle.com/store?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get the humble store partner link'),
(5, 'monthly', 'Humble bundle monthly : https://www.humblebundle.com/monthly?partner=novaevermoon', 'everyone', 10000, 1, 0, 'Get humble monthly link'),
(6, 'crystals', 'Crystals are the channel\'s currency, you earn them while watching the stream and by following, hosting, subscribing etc. You can use these to buy neat rewards ! Leaderboard : https://streamlabs.com/novaevermoon#/loyalty', 'everyone', 10000, 1, 0, 'Get crystal infos'),
(7, 'lurk', 'is going into lurk mode ! Thank you ♡', 'everyone', 10000, 1, 0, ''),
(8, 'discord', 'Wanna be part of the cool kids and hang out after stream? You can get access to my discord either by subbing or with crystals (!crystals for more info :3 )', 'everyone', 10000, 1, 0, 'get discord info');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `command`
--
ALTER TABLE `command`
  ADD PRIMARY KEY (`command_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `command`
--
ALTER TABLE `command`
  MODIFY `command_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
