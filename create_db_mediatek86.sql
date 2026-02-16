-- Script SQL de création de la BDD MediaTek86
-- A exécuter dans phpMyAdmin sous WampServer

CREATE DATABASE IF NOT EXISTS mediatek86;
USE mediatek86;

-- Création de l'utilisateur
CREATE USER IF NOT EXISTS 'admin_mediatek86'@'localhost' IDENTIFIED BY 'AdminMdk86!';
GRANT ALL PRIVILEGES ON mediatek86.* TO 'admin_mediatek86'@'localhost';
FLUSH PRIVILEGES;

-- Table service
CREATE TABLE IF NOT EXISTS service (
    idservice INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL
) ENGINE=InnoDB;

-- Table personnel
CREATE TABLE IF NOT EXISTS personnel (
    idpersonnel INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    tel VARCHAR(15) NOT NULL,
    mail VARCHAR(255) NOT NULL,
    idservice INT NOT NULL,
    CONSTRAINT fk_personnel_service FOREIGN KEY (idservice) REFERENCES service (idservice)
) ENGINE=InnoDB;

-- Table motif
CREATE TABLE IF NOT EXISTS motif (
    idmotif INT AUTO_INCREMENT PRIMARY KEY,
    libelle VARCHAR(50) NOT NULL
) ENGINE=InnoDB;

-- Table absence (clé primaire composée : idpersonnel + datedebut)
CREATE TABLE IF NOT EXISTS absence (
    idpersonnel INT NOT NULL,
    datedebut DATETIME NOT NULL,
    datefin DATETIME NOT NULL,
    idmotif INT NOT NULL,
    PRIMARY KEY (idpersonnel, datedebut),
    CONSTRAINT fk_absence_personnel FOREIGN KEY (idpersonnel) REFERENCES personnel (idpersonnel) ON DELETE CASCADE,
    CONSTRAINT fk_absence_motif FOREIGN KEY (idmotif) REFERENCES motif (idmotif)
) ENGINE=InnoDB;

-- Table responsable (1 seule ligne, pas d'identifiant)
CREATE TABLE IF NOT EXISTS responsable (
    login VARCHAR(64) NOT NULL,
    pwd VARCHAR(64) NOT NULL
) ENGINE=InnoDB;

-- ===== INSERTION DES DONNEES =====

-- Responsable (mdp: admin, haché en SHA-256)
INSERT INTO responsable (login, pwd) VALUES ('admin', SHA2('admin', 256));

-- Services
INSERT INTO service (idservice, nom) VALUES
(1, 'administratif'),
(2, 'médiation culturelle'),
(3, 'prêt');

-- Motifs d'absence
INSERT INTO motif (idmotif, libelle) VALUES
(1, 'vacances'),
(2, 'maladie'),
(3, 'motif familial'),
(4, 'congé parental');

-- Personnel (10 exemples)
INSERT INTO personnel (nom, prenom, tel, mail, idservice) VALUES
('Dupont', 'Jean', '0601020304', 'jean.dupont@mediatek86.fr', 1),
('Martin', 'Sophie', '0611223344', 'sophie.martin@mediatek86.fr', 2),
('Leroy', 'Marc', '0655443322', 'marc.leroy@mediatek86.fr', 3),
('Roux', 'Emma', '0766554433', 'emma.roux@mediatek86.fr', 3),
('Moreau', 'Lucas', '0788990011', 'lucas.moreau@mediatek86.fr', 1),
('Simon', 'Chloé', '0612345678', 'chloe.simon@mediatek86.fr', 2),
('Laurent', 'Paul', '0698765432', 'paul.laurent@mediatek86.fr', 3),
('Lefevre', 'Julie', '0711121314', 'julie.lefevre@mediatek86.fr', 3),
('Michel', 'Thomas', '0755565758', 'thomas.michel@mediatek86.fr', 2),
('Garcia', 'Lea', '0622334455', 'lea.garcia@mediatek86.fr', 1);

-- Absences de test
INSERT INTO absence (idpersonnel, datedebut, datefin, idmotif) VALUES
(1, '2025-01-06 00:00:00', '2025-01-10 23:59:59', 1),
(1, '2025-03-17 00:00:00', '2025-03-18 23:59:59', 2),
(2, '2025-02-03 00:00:00', '2025-02-07 23:59:59', 1),
(3, '2025-01-20 00:00:00', '2025-01-20 23:59:59', 3),
(3, '2025-04-07 00:00:00', '2025-04-11 23:59:59', 1),
(4, '2025-02-10 00:00:00', '2025-02-14 23:59:59', 2),
(5, '2025-03-03 00:00:00', '2025-03-05 23:59:59', 1),
(6, '2025-01-13 00:00:00', '2025-01-17 23:59:59', 4),
(6, '2025-05-12 00:00:00', '2025-05-16 23:59:59', 1),
(7, '2025-02-24 00:00:00', '2025-02-28 23:59:59', 2),
(8, '2025-04-14 00:00:00', '2025-04-15 23:59:59', 3),
(9, '2024-11-04 00:00:00', '2025-02-28 23:59:59', 4),
(10, '2025-03-10 00:00:00', '2025-03-14 23:59:59', 1),
(2, '2025-04-22 00:00:00', '2025-04-25 23:59:59', 2),
(5, '2025-05-05 00:00:00', '2025-05-06 23:59:59', 3);
