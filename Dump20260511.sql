CREATE DATABASE  IF NOT EXISTS `scientific_foundation` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `scientific_foundation`;
-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: scientific_foundation
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
-- Table structure for table `directions`
--

DROP TABLE IF EXISTS `directions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `directions` (
  `id_direction` varchar(45) NOT NULL,
  `name_direction` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_direction`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `directions`
--

LOCK TABLES `directions` WRITE;
/*!40000 ALTER TABLE `directions` DISABLE KEYS */;
INSERT INTO `directions` VALUES ('BIO','Biologie'),('MATH','Mathematics'),('PHY','Physics');
/*!40000 ALTER TABLE `directions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grants`
--

DROP TABLE IF EXISTS `grants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grants` (
  `id_grants` int NOT NULL,
  `id_scient` int NOT NULL,
  `id_direction` varchar(45) NOT NULL,
  `name_theme` varchar(45) DEFAULT NULL,
  `date_start` datetime DEFAULT NULL,
  `date_end` datetime DEFAULT NULL,
  `organization` varchar(45) DEFAULT NULL,
  `summa` int DEFAULT NULL,
  PRIMARY KEY (`id_grants`,`id_scient`,`id_direction`),
  KEY `frg_key_one_idx` (`id_scient`),
  KEY `frg_key_two_idx` (`id_direction`),
  CONSTRAINT `frg_key_one` FOREIGN KEY (`id_scient`) REFERENCES `scientists` (`id_scient`),
  CONSTRAINT `frg_key_two` FOREIGN KEY (`id_direction`) REFERENCES `directions` (`id_direction`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grants`
--

LOCK TABLES `grants` WRITE;
/*!40000 ALTER TABLE `grants` DISABLE KEYS */;
INSERT INTO `grants` VALUES (0,3,'BIO','Географий','2024-07-21 00:00:00','2024-09-03 00:00:00','БГПУ',120000),(1001,1,'PHY','Космос','2020-12-23 00:00:00','2021-01-28 00:00:00','МГПУ',100000),(1002,2,'BIO','Земля','2022-05-15 00:00:00','2022-07-02 00:00:00','СГУИЛ',800000),(1003,1,'MATH','Геометрия','2023-07-21 00:00:00','2023-09-12 00:00:00','МГПУ',50000);
/*!40000 ALTER TABLE `grants` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `grants_BEFORE_INSERT` BEFORE INSERT ON `grants` FOR EACH ROW BEGIN
	IF new.date_start >= new.date_end THEN
		SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ошибка: дата начала должна быть раньше даты окончания';
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `grants_BEFORE_UPDATE` BEFORE UPDATE ON `grants` FOR EACH ROW BEGIN
	IF NEW.date_start >= OLD.date_end THEN
		SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ошибка: дата начала должна быть раньше даты окончания';
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `partisipant`
--

DROP TABLE IF EXISTS `partisipant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `partisipant` (
  `id_grant` int NOT NULL,
  `id_scient` int NOT NULL,
  PRIMARY KEY (`id_grant`,`id_scient`),
  KEY `frg_parti_two_idx` (`id_scient`),
  CONSTRAINT `frg_parti_one` FOREIGN KEY (`id_grant`) REFERENCES `grants` (`id_grants`),
  CONSTRAINT `frg_parti_two` FOREIGN KEY (`id_scient`) REFERENCES `scientists` (`id_scient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partisipant`
--

LOCK TABLES `partisipant` WRITE;
/*!40000 ALTER TABLE `partisipant` DISABLE KEYS */;
INSERT INTO `partisipant` VALUES (1001,1),(1003,1),(1002,2);
/*!40000 ALTER TABLE `partisipant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scientists`
--

DROP TABLE IF EXISTS `scientists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scientists` (
  `id_scient` int NOT NULL,
  `fio_scient` varchar(45) DEFAULT NULL,
  `date` date DEFAULT NULL,
  `degree` varchar(45) DEFAULT NULL,
  `title` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_scient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scientists`
--

LOCK TABLES `scientists` WRITE;
/*!40000 ALTER TABLE `scientists` DISABLE KEYS */;
INSERT INTO `scientists` VALUES (1,'Олегов Олег Олегович','1998-12-12','Доктор наук','Профессор'),(2,'Иванов Иван Иванович','1987-05-02','Доктор наук','Профессор'),(3,'Сергеев Сергей Сергеевич','2000-11-06','Кандидат наук','Доцент ');
/*!40000 ALTER TABLE `scientists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'scientific_foundation'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-11 11:15:05
