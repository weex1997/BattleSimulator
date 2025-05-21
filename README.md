# BattleSimulator
Installation GUIDE
Introduction
The game is about two opposing teams - a blue team and a red team. The red team's lineup is randomly generated, while the blue team's
lineup is chosen by the player to create the best possible lineup to defeat the red team.
Technical Documentation
Initial Setup
Spawning: The team members will appear on a 3x3 grid determined by the team's lineup, you can add team lineup data by adding a new
scriptable object. and the system will spawn the units depending on the data. for the prototype, I create 4 lineups.
So, I create a scriptable object have a list of vector2 to store x and y for each unit then in baker script I store the x , y in DynamicBuffer
as float2 to use it because ECS not support List , and DynamicBuffer works instead
