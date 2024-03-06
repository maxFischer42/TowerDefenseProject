# Tower Defense Game (Temporary Title)

This repository holds the base for my Tower Defense Project; my own take on a once-popular game genre. This project has primarily been developed in-between my classes in my final semester of college as a way to put what I've learned to the test in a different way than I have in the past.

Based on games such as the Bloons TD series and Pokemon Tower Defense, this Tower Defense Game aims to recreate the kind of tower defense games that were popular in the early 2010s. 

## Gameplay
### Warriors, Heroes and Upgrades
When starting a session, the player will begin with a set amount of money. This money can be used to purchase a "Warrior", "Warrior Upgrade", or a "Field Enhancement". Warriors are the "towers" of this game, characters that serve specific purposes to fend off enemies. Warriors come in all sorts of forms and can gain different attributes when Warrior Upgrades are purchased. Warrior Upgrades can be purchased when selecting a Warrior that has already been bought. Warrior upgrades are unlocked per warrior at specific level intervals that are specific to each warrior. 

There also exists a unique kind of warrior titled "Heroes". Unlike normal warriors, only a single of each hero can be used per session. If that Hero is defeated, they cannot be used again in a given session.

Upgrades can change the stats of a warrior, such as changing their range, their firerate, the object they spawn to attack, and many other attributes. 

Field Enhancements are temporary purchases to assist warriors already on the map, such as a healing field to recover damage heroes have taken in a specific radius, or a damage boost field to temporarily increase the damage of a group of warriors.

### Enemies
The lose condition of a session is to run out of health. Health depletes when an enemy reaches the end of the course. Enemies come in all shapes and sizes, sometimes in multiple layers as well. Multi-layered enemies will spawn a sub-enemy on death or on some other kind of action. 

Some enemies have special effects that can change the flow of battle, such as: spawning a field that increases the speed of enemies moving over it, stunning nearby heroes on death, stopping in the middle of the track and summoning a massive enemy, and many many others. Enemies can also attack warriors along the field. When a warrior loses all of it's health, it is removed from the field. 

Some enemies have special ways of eliminating warriors, such as possessing a warrior once it has defeated it, creating an enemy out of the player's own unit. 

There also exist enemies in the form of Boss Enemies, enemies with special attributes that aim to really challenge the player's warrior layout and upgrade strategy.

### Win Condition
Enemies will spawn in waves of 1 or more per session. The player will achieve a win condition if they complete the final wave with health still remaining.

## Object List
The following are all of the current existing objects per category that are fully functional in-game, primarily the player's units, and potential enemy units.
### Warriors
- Archer
- Slayer
- Cleric
- Healing Mage

### Heroes

### Enemies
- Barbarian
- Carriage
- Cavalry
- Grunt
- Shadow Summoner
- Shadow Striker
- Shadow Fragments
- Bothered Slime
- Pestered Slime
- Angered Slime
- Royal Slime
- Thunder Slime
- Fire Slime
- Poison Slime
- Grass Slime

### Bosses
- Shadow Beast
- Slime Princess
- Slime Queen
