CREATE DATABASE  IF NOT EXISTS `metalurgica` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `metalurgica`;
-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: localhost    Database: metalurgica
-- ------------------------------------------------------
-- Server version	8.0.43

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
-- Table structure for table `clientes`
--

DROP TABLE IF EXISTS `clientes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientes` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `apellido` varchar(30) DEFAULT NULL,
  `tipo` enum('PERSONA','EMPRESA') NOT NULL DEFAULT 'PERSONA',
  `telefono` varchar(25) DEFAULT NULL,
  `cuit` varchar(15) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  `direccion` varchar(100) DEFAULT NULL,
  `fechaRegistrado` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_clientes_cuit` (`cuit`),
  KEY `idx_clientes_nombre` (`nombre`),
  KEY `idx_clientes_apellido` (`apellido`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--

LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES (1,'Manuel','Perez','PERSONA','1234567890','20-34567890-1',1,'Avenida Siempreviva 123','2026-10-08'),(2,'prueba1','apellido1','PERSONA','1234-567809','20-12345678-1',1,'la casa esa con puertas y ventanas 123','2026-10-08'),(3,'prueba2','apellido2','PERSONA','1234-567891','20-12345678-2',1,'otra direccion 345','2026-10-08'),(20,'carlitos','gutierrez','PERSONA',NULL,NULL,0,'una casa 123','2026-11-03'),(22,'nombre','apellido','PERSONA','','',0,'direccion 321','2026-11-03'),(23,'manu','unam','PERSONA',NULL,NULL,1,NULL,'2026-11-03'),(24,'manuela','aleunam','PERSONA',NULL,NULL,1,NULL,'2026-11-03'),(25,'manuelinho','dos santos da silva','PERSONA',NULL,NULL,1,NULL,'2026-11-03');
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalles`
--

DROP TABLE IF EXISTS `detalles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalles` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `trabajoId` bigint unsigned NOT NULL,
  `tipoOperacionId` tinyint unsigned DEFAULT NULL,
  `materialId` smallint unsigned NOT NULL,
  `largo` decimal(8,2) DEFAULT NULL,
  `ancho` decimal(8,2) DEFAULT NULL,
  `cantidad` smallint unsigned DEFAULT NULL,
  `kgPlegados` decimal(6,2) DEFAULT NULL,
  `precioMaterial` decimal(10,2) DEFAULT NULL,
  `precioTipoOperacion` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `trabajoId` (`trabajoId`),
  KEY `tipoOperacionId` (`tipoOperacionId`),
  KEY `materialId` (`materialId`),
  CONSTRAINT `detalles_ibfk_1` FOREIGN KEY (`trabajoId`) REFERENCES `trabajos` (`id`),
  CONSTRAINT `detalles_ibfk_2` FOREIGN KEY (`tipoOperacionId`) REFERENCES `tipo_operaciones` (`id`),
  CONSTRAINT `detalles_ibfk_3` FOREIGN KEY (`materialId`) REFERENCES `materiales` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalles`
--

LOCK TABLES `detalles` WRITE;
/*!40000 ALTER TABLE `detalles` DISABLE KEYS */;
INSERT INTO `detalles` VALUES (4,1,1,8,820.00,520.00,3,21.11,1.00,1.00),(5,1,3,9,1000.00,850.00,5,55.25,1.00,1.00),(6,1,4,2,500.00,250.00,1,9.43,1.00,1.00),(9,3,1,8,700.00,400.00,1,4.62,1.00,1.00),(10,3,3,19,900.00,500.00,1,1.89,1.00,1.00),(12,4,3,18,1000.00,600.00,10,34.80,1.00,1.00);
/*!40000 ALTER TABLE `detalles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fotos`
--

DROP TABLE IF EXISTS `fotos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fotos` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `ruta` varchar(255) NOT NULL,
  `trabajoId` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `trabajoId` (`trabajoId`),
  CONSTRAINT `fotos_ibfk_1` FOREIGN KEY (`trabajoId`) REFERENCES `trabajos` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fotos`
--

LOCK TABLES `fotos` WRITE;
/*!40000 ALTER TABLE `fotos` DISABLE KEYS */;
INSERT INTO `fotos` VALUES (1,'/Uploads\\Fotos\\1\\foto_4c54b781-f5b7-4819-874e-9d0bc651862d.jpg',1),(3,'/Uploads\\Fotos\\1\\foto_8782b7a8-5d40-485a-b378-e02d75c7af2b.jpg',1),(6,'/Uploads\\Fotos\\3\\foto_fc4af537-d399-4fdb-8dce-47ca09a16302.jpg',3);
/*!40000 ALTER TABLE `fotos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `materiales`
--

DROP TABLE IF EXISTS `materiales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `materiales` (
  `id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `tipo` enum('SIN_TIPO','CHAPA','CAÑO','PINTURA') NOT NULL DEFAULT 'SIN_TIPO',
  `descripcion` varchar(255) DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL DEFAULT '0.00',
  `precioXkg` decimal(8,2) DEFAULT NULL,
  `pesoXm2` decimal(7,3) DEFAULT NULL,
  `color` varchar(50) DEFAULT NULL,
  `litros` decimal(5,2) DEFAULT NULL,
  `eliminado` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materiales`
--

LOCK TABLES `materiales` WRITE;
/*!40000 ALTER TABLE `materiales` DISABLE KEYS */;
INSERT INTO `materiales` VALUES (1,'CHAPA','Chapa Acero al Carbono 1/2',1.00,3800.00,100.480,NULL,NULL,0),(2,'CHAPA','Chapa Acero al Carbono 3/8',1.00,4000.00,75.400,NULL,NULL,0),(3,'CHAPA','Chapa Acero al Carbono 5/16',1.00,4000.00,62.800,NULL,NULL,0),(4,'CHAPA','Chapa Acero al Carbono 1/4',1.00,4400.00,50.240,NULL,NULL,0),(5,'CHAPA','Chapa Acero al Carbono 3/16',1.00,4700.00,37.580,NULL,NULL,0),(6,'CHAPA','Chapa Acero al Carbono 1/8',1.00,3900.00,25.120,NULL,NULL,0),(7,'CHAPA','Chapa Acero al Carbono 12',1.00,3700.00,21.666,NULL,NULL,0),(8,'CHAPA','Chapa Acero al Carbono 14',1.00,3900.00,16.500,NULL,NULL,0),(9,'CHAPA','Chapa Acero al Carbono 16',1.00,4100.00,13.000,NULL,NULL,0),(10,'CHAPA','Chapa Acero al Carbono 18',1.00,4500.00,10.000,NULL,NULL,0),(11,'CHAPA','Chapa Acero al Carbono 20',1.00,4800.00,7.120,NULL,NULL,0),(12,'CHAPA','Chapa Acero al Carbono 22',1.00,4800.00,5.700,NULL,NULL,0),(13,'CHAPA','Chapa Acero al Carbono 24',1.00,4800.00,4.400,NULL,NULL,0),(14,'CHAPA','Chapa Galvanizada 14',1.00,4500.00,16.080,NULL,NULL,0),(15,'CHAPA','Chapa Galvanizada 16',1.00,4600.00,12.730,NULL,NULL,0),(16,'CHAPA','Chapa Galvanizada 18',1.00,4700.00,10.100,NULL,NULL,0),(17,'CHAPA','Chapa Galvanizada 20',1.00,4700.00,7.350,NULL,NULL,0),(18,'CHAPA','Chapa Galvanizada 22',1.00,4800.00,5.800,NULL,NULL,0),(19,'CHAPA','Chapa Galvanizada 24',1.00,4900.00,4.200,NULL,NULL,0),(20,'CHAPA','Chapa Galvanizada 27',1.00,4500.00,3.350,NULL,NULL,0),(21,'CHAPA','Chapa Galvanizada 30',1.00,4500.00,2.500,NULL,NULL,0),(22,'CHAPA','Chapa Prepintada Negra 24',1.00,7100.00,4.200,NULL,NULL,0),(23,'CAÑO','Caño Estructural Cuadrado 100x100x1.6 - 31.5kg - 6mts',1.00,NULL,NULL,NULL,NULL,0),(24,'CAÑO','Caño Estructural Cuadrado 100x100x3.2 - 63kg - 6mts',1.00,NULL,NULL,NULL,NULL,0),(25,'CAÑO','Caño Estructural Rectangular 40x80x1.6 - 18kg - 6mts',1.00,NULL,NULL,NULL,NULL,0),(26,'CAÑO','Caño Estructural Rectangular 40x80x3.2 - 36kg - 6mts',1.00,NULL,NULL,NULL,NULL,0),(27,'CAÑO','Caño Estructural Rectangular 10x30x1.6 - 6kg - 6mts',1.00,NULL,NULL,NULL,NULL,0),(28,'PINTURA','Antioxidante Negro',1.00,NULL,NULL,'Negro',NULL,0),(29,'PINTURA','Antioxidante Rojo',1.00,NULL,NULL,'Rojo',NULL,0),(30,'CHAPA','chapita chapota',2.00,100.00,2.000,NULL,NULL,1);
/*!40000 ALTER TABLE `materiales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_operaciones`
--

DROP TABLE IF EXISTS `tipo_operaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_operaciones` (
  `id` tinyint unsigned NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `costoFijo` decimal(8,2) unsigned NOT NULL DEFAULT '0.00',
  `eliminado` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_tipos_actividades_nombre` (`nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_operaciones`
--

LOCK TABLES `tipo_operaciones` WRITE;
/*!40000 ALTER TABLE `tipo_operaciones` DISABLE KEYS */;
INSERT INTO `tipo_operaciones` VALUES (1,'Corte',1.00,0),(2,'Plegado',1.00,0),(3,'Corte y Plegado',1.00,0),(4,'Corte Plasma',1.00,0),(5,'Cilindrado',1.00,0),(6,'prueba',3.00,1);
/*!40000 ALTER TABLE `tipo_operaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trabajos`
--

DROP TABLE IF EXISTS `trabajos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trabajos` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `fechaSolicitado` date NOT NULL DEFAULT (curdate()),
  `fechaIniciado` date DEFAULT NULL,
  `fechaFinalizado` date DEFAULT NULL,
  `fechaRetirado` date DEFAULT NULL,
  `importe` decimal(11,2) NOT NULL DEFAULT '0.00',
  `descripcion` varchar(512) DEFAULT NULL,
  `estado` enum('PRESUPUESTO','ACEPTADO','RECHAZADO') DEFAULT 'PRESUPUESTO',
  `observacion` varchar(500) DEFAULT NULL,
  `clienteId` int unsigned NOT NULL,
  `registradorId` int unsigned DEFAULT NULL,
  `anulado` tinyint(1) NOT NULL DEFAULT '0',
  `anuladorId` int unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_cliente` (`clienteId`),
  KEY `fk_registrador` (`registradorId`),
  KEY `fk_trabajos_anulador` (`anuladorId`),
  CONSTRAINT `fk_cliente` FOREIGN KEY (`clienteId`) REFERENCES `clientes` (`id`),
  CONSTRAINT `fk_registrador` FOREIGN KEY (`registradorId`) REFERENCES `usuarios` (`id`),
  CONSTRAINT `fk_trabajos_anulador` FOREIGN KEY (`anuladorId`) REFERENCES `usuarios` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trabajos`
--

LOCK TABLES `trabajos` WRITE;
/*!40000 ALTER TABLE `trabajos` DISABLE KEYS */;
INSERT INTO `trabajos` VALUES (1,'2026-10-14',NULL,NULL,NULL,110444.00,NULL,'ACEPTADO',NULL,1,1,0,NULL),(3,'2026-10-18','2026-10-19',NULL,NULL,27279.00,'Canaletas y corte ch 14','PRESUPUESTO',NULL,3,1,0,NULL),(4,'2026-11-01',NULL,NULL,NULL,16704.00,'Cenefas','ACEPTADO',NULL,2,1,0,NULL);
/*!40000 ALTER TABLE `trabajos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(20) NOT NULL,
  `pwd` varchar(255) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `rol` enum('ADMIN','EMPLEADO') NOT NULL DEFAULT 'EMPLEADO',
  `nombre` varchar(30) NOT NULL,
  `apellido` varchar(30) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  `avatar` varchar(255) DEFAULT NULL,
  `fechaRegistrado` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_usuarios_username` (`username`),
  UNIQUE KEY `uq_usuarios_avatar` (`avatar`),
  UNIQUE KEY `uq_usuarios_email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'manuel','peVThjUfUu01fyGdtcSAFnYxo2TjLo2aQ3hPuZEUoms=',NULL,'ADMIN','Manuel','Gutierrez',1,'/Uploads\\Avatares\\avatar_1_639261314299083838.jpg','2026-10-08'),(2,'user2','peVThjUfUu01fyGdtcSAFnYxo2TjLo2aQ3hPuZEUoms=',NULL,'EMPLEADO','marcela','alonso',1,NULL,'2026-10-08');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-03 19:18:20
