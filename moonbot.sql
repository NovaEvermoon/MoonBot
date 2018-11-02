-- phpMyAdmin SQL Dump
-- version 4.8.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 02, 2018 at 04:24 PM
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
  `command_description` varchar(500) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `command`
--

INSERT INTO `command` (`command_id`, `command_keyword`, `command_message`, `command_userLevel`, `command_cooldown`, `command_status`, `command_description`) VALUES
(1, 'nova', 'Follow me on social media! ♡ https://linktr.ee/nova.evermoon', 'everyone', 30000, 1, 'Display Nova\'s social media');

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
  MODIFY `command_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
