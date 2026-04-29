CREATE DATABASE  IF NOT EXISTS `computer_club` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_ru_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `computer_club`;
-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: computer_club
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bookings`
--

DROP TABLE IF EXISTS `bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookings` (
  `booking_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int unsigned NOT NULL,
  `zone_id` int unsigned NOT NULL,
  `tariff_id` int unsigned NOT NULL,
  `booking_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `start_time` datetime NOT NULL,
  `end_time` datetime NOT NULL,
  `total_price` decimal(10,2) NOT NULL,
  `status` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  PRIMARY KEY (`booking_id`),
  KEY `frg_booking_one_idx` (`user_id`),
  KEY `frg_booking_two_idx` (`zone_id`),
  KEY `frg_booking_three_idx` (`tariff_id`),
  CONSTRAINT `frg_booking_one` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE,
  CONSTRAINT `frg_booking_three` FOREIGN KEY (`tariff_id`) REFERENCES `tariffs` (`tariff_id`) ON UPDATE CASCADE,
  CONSTRAINT `frg_booking_two` FOREIGN KEY (`zone_id`) REFERENCES `zones` (`zone_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookings`
--

LOCK TABLES `bookings` WRITE;
/*!40000 ALTER TABLE `bookings` DISABLE KEYS */;
INSERT INTO `bookings` VALUES (1,2,1,1,'2026-04-23 17:11:32','2023-10-25 12:00:00','2023-10-25 14:00:00',500.00,'confirmed');
/*!40000 ALTER TABLE `bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int unsigned NOT NULL AUTO_INCREMENT,
  `name_category` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `describe_category` varchar(255) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'VIP-залы','Комфортные кресла и топовое железо'),(2,'Общий зал','Стандартные места по доступной цене');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `club_has_categories`
--

DROP TABLE IF EXISTS `club_has_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `club_has_categories` (
  `category_id` int unsigned NOT NULL,
  `club_id` int unsigned NOT NULL,
  PRIMARY KEY (`category_id`,`club_id`),
  KEY `frg_club_category_two_idx` (`club_id`),
  CONSTRAINT `frg_club_category_one` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `frg_club_category_two` FOREIGN KEY (`club_id`) REFERENCES `clubs` (`club_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `club_has_categories`
--

LOCK TABLES `club_has_categories` WRITE;
/*!40000 ALTER TABLE `club_has_categories` DISABLE KEYS */;
INSERT INTO `club_has_categories` VALUES (1,1),(2,1);
/*!40000 ALTER TABLE `club_has_categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clubs`
--

DROP TABLE IF EXISTS `clubs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clubs` (
  `club_id` int unsigned NOT NULL AUTO_INCREMENT,
  `manager_id` int unsigned NOT NULL,
  `name_club` varchar(65) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `describe_club` varchar(255) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `address_club` varchar(100) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `phone_club` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `email_club` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `main_photo_club` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  PRIMARY KEY (`club_id`),
  UNIQUE KEY `name_club_UNIQUE` (`name_club`),
  KEY `frg_club_one_idx` (`manager_id`),
  CONSTRAINT `frg_club_one` FOREIGN KEY (`manager_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clubs`
--

LOCK TABLES `clubs` WRITE;
/*!40000 ALTER TABLE `clubs` DISABLE KEYS */;
INSERT INTO `clubs` VALUES (1,1,'CyberSphere','Лучший клуб в центре','ул. Ленина, 10','777-888','cyber@club.ru','photo1.jpg');
/*!40000 ALTER TABLE `clubs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `review_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int unsigned DEFAULT NULL,
  `club_id` int unsigned NOT NULL,
  `rating` int NOT NULL,
  `comment` varchar(255) COLLATE utf8mb4_ru_0900_ai_ci DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `status` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  PRIMARY KEY (`review_id`),
  KEY `frg_review_one_idx` (`user_id`),
  KEY `frg_review_two_idx` (`club_id`),
  CONSTRAINT `frg_review_one` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE,
  CONSTRAINT `frg_review_two` FOREIGN KEY (`club_id`) REFERENCES `clubs` (`club_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (1,2,1,5,'Всё супер, мощные компы!','2026-04-23 17:11:37','published');
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tariffs`
--

DROP TABLE IF EXISTS `tariffs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tariffs` (
  `tariff_id` int unsigned NOT NULL AUTO_INCREMENT,
  `zone_id` int unsigned NOT NULL,
  `name_tariff` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `price_per_hour` decimal(10,2) NOT NULL,
  `start_time` datetime NOT NULL,
  `end_time` datetime NOT NULL,
  PRIMARY KEY (`tariff_id`),
  KEY `frg_tariff_one_idx` (`zone_id`),
  CONSTRAINT `frg_tariff_one` FOREIGN KEY (`zone_id`) REFERENCES `zones` (`zone_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tariffs`
--

LOCK TABLES `tariffs` WRITE;
/*!40000 ALTER TABLE `tariffs` DISABLE KEYS */;
INSERT INTO `tariffs` VALUES (1,1,'Дневной VIP',250.00,'2023-10-01 10:00:00','2023-10-01 18:00:00'),(2,2,'Ночной стандарт',150.00,'2023-10-01 22:00:00','2023-10-02 08:00:00');
/*!40000 ALTER TABLE `tariffs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int unsigned NOT NULL AUTO_INCREMENT,
  `login` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `password` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `email_user` varchar(100) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `phone_user` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `full_name` varchar(100) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `role_user` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `registr_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_active` tinyint NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin_max','pass123','max@mail.ru','89001112233','Максим Менеджеров','manager','2026-04-23 17:10:00',1),(2,'ivan_777','qwerty','ivan@mail.ru','89004445566','Иван Иванов','client','2026-04-23 17:10:00',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zones`
--

DROP TABLE IF EXISTS `zones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zones` (
  `zone_id` int unsigned NOT NULL AUTO_INCREMENT,
  `club_id` int unsigned NOT NULL,
  `name_zone` varchar(45) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `describe_zone` varchar(100) COLLATE utf8mb4_ru_0900_ai_ci NOT NULL,
  `capacity` int NOT NULL,
  PRIMARY KEY (`zone_id`),
  KEY `frg_zone_one_idx` (`club_id`),
  CONSTRAINT `frg_zone_one` FOREIGN KEY (`club_id`) REFERENCES `clubs` (`club_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_ru_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zones`
--

LOCK TABLES `zones` WRITE;
/*!40000 ALTER TABLE `zones` DISABLE KEYS */;
INSERT INTO `zones` VALUES (1,1,'Bootcamp A','Для командных тренировок',5),(2,1,'Main Hall','Большой зал',30);
/*!40000 ALTER TABLE `zones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'computer_club'
--

--
-- Dumping routines for database 'computer_club'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-04-30  2:07:16
