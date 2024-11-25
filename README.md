# TourGuide Application

TourGuide est une application simulant un service de recommandation de voyages et d'attractions touristiques. Elle intègre des fonctionnalités de gestion d'utilisateurs, de localisation, et de récompenses.

---

## **Table des matières**
- [Fonctionnalités](#fonctionnalités)
- [Architecture](#architecture)
- [Prérequis](#prérequis)
- [Installation](#installation)

---

## **Fonctionnalités**
- **Localisation des utilisateurs :**
  - Obtention des coordonnées GPS d'un utilisateur via `GpsUtil`.

- **Recommandation d'attractions :**
  - Liste d'attractions populaires avec leurs coordonnées géographiques.
  - Calcul des distances entre l'utilisateur et les attractions.

- **Gestion des récompenses :**
  - Attribution de points de récompense pour les visites d'attractions.

---

## **Architecture**
L'application est organisée autour de plusieurs modules principaux :

- **GpsUtil** :
  - Fournit les coordonnées géographiques des utilisateurs et des attractions.

- **RewardsService** :
  - Calcule les points de récompense pour les utilisateurs visitant des attractions.

- **Users** :
  - Contient les préférences et informations des utilisateurs.

---

## **Prérequis**
- **.NET 6 ou supérieur** installé sur votre machine.

---

## **Installation**
1. Clonez le dépôt Git :
   ```bash
   git clone https://github.com/RenatoSclr/backend-dot-NET_P8.git
   ```
