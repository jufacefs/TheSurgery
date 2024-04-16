INCLUDE DialoguesInTailorShop
INCLUDE DialoguesOnStreet
INCLUDE AptDialogues
INCLUDE DialoguesAtPort
INCLUDE DialoguesPolice
INCLUDE Bedroom
INCLUDE InBoat
INCLUDE UnionBuilding
INCLUDE StartScreen
INCLUDE Endings










/*
Ink Cheat sheet

=== cat ===
marks a "knot", which is a dialogue, monologue, or cutscene accessible through in-game interaction. They are the entry point to any dialogues.

= cat_angry
marks a "stitch", or a passage within the knot that can be referenced. 

* [normal option] can be choosen only once
+ [sticky option] won't diappear (useful for navigation and interaction start/end)

-> divert aka links to stitch or knot

//single line comment

//choice conditioned to visited passage
* {cat_angry} [Pet the cat]


//conditions if/else
{
-passage_title:
    do this
-VARIABLE_NAME >= 2:
    do that
-else:
    otherwise
}

*/


EXTERNAL teleport(objectName)
EXTERNAL activate(objectName)
EXTERNAL deactivate(objectName)
EXTERNAL gameEvent(eventName)
EXTERNAL LoseScore(number)
EXTERNAL PlayEffect()
EXTERNAL StopEffect()


=== function teleport(x) ===
// Usually external functions can only return placeholder
// results, otherwise they'd be defined in ink!
~ return 1
=== function activate(x) ===
~ return 1
=== function deactivate(x) ===
~ return 1
=== function gameEvent(x) ===
~ return 1
=== function LoseScore(x) ===
~ return 1

=== function PlayEffect() ===
~ return 1
=== function StopEffect() ===
~ return 1

//I like to use allcaps for global variable to distinguish them from knots ids
VAR KEY = false
VAR LEVEL = 0









//________________--------_________________
//_________________________________________//________________--------_________________
//_________________________________________

=== u_fratToStreet ===
~ activate("upper_streets")
~ deactivate("upper_Frats")
~ teleport("u_streetToFrat")
->END

=== u_streetToRes ===
~ activate("upper_restaurant")
~ deactivate("upper_streets")
~ teleport("res_frontDoor")
->END

=== u_RestToStreet ===
~ activate("upper_streets")
~ deactivate("upper_restaurants")
~ teleport("u_streetToRes")
->END

=== u_RestToPark ===
~ activate("upper_park")
~ deactivate("upper_restaurant")
~ teleport("parkEntrance1")
->END


=== u_parkToRest ===
~ activate("upper_restaurant")
~ deactivate("upper_park")
~ teleport("res_backDoor")
->END


=== u_streetToPolice ===
~ activate("upper_police")
~ deactivate("upper_streets")
~ teleport("u_policeToStreet")
->END


=== u_policeToStreet ===
~ activate("upper_streets")
~ deactivate("upper_police")
~ teleport("u_streetToPolice")
->END



=== u_streetToApt===
~ activate("upper_apt")
~ deactivate("upper_streets")
~ teleport("u_aptFrontDoor")
->END

=== u_aptToStreet ===
~ activate("upper_streets")
~ deactivate("upper_apt")
~ teleport("u_streetToApt")
->END


=== u_aptToPark ===
~ activate("upper_park")
~ deactivate("upper_apt")
~ teleport("parkEntrance2")
->END

=== u_parkToApt ===
~ activate("upper_apt")
~ deactivate("upper_park")
~ teleport("u_aptBackDoor")
->END


=== u_streetToPark ===
~ activate("upper_park")
~ deactivate("upper_streets")
~ teleport("parkEntrance2")
->END



=== u_streetToTailor ===
~ activate("upper_tailor")
~ deactivate("upper_streets")
~ teleport("u_tailorToStreet")
->END

=== u_tailorToStreet ===
~ activate("upper_streets")
~ deactivate("upper_tailor")
~ teleport("u_streetTo_Tailor")
->END


//________________--------_________________
//_________________________________________//________________--------_________________
//_________________________________________



=== door ===
{
- KEY == true: -> open
- else: -> locked
}

= open
The door is open!
~ teleport("bluePortal")
->END

= locked
You need a key to open the door
->END

=== key ===
Picking up the key
~ deactivate("key")
~ KEY = true
->END

=== exit ===
The End
~ gameEvent("restart")

->END

//example of "room" change
=== pinkToBlue ===
~ activate("blueDesert")
~ deactivate("pinkDesert")
~ teleport("pinkDoor")
->END

=== blueToPink ===
~ activate("pinkDesert")
~ deactivate("blueDesert")
~ teleport("blueDoor")
->END



=== streetToTailorShop ===
~ activate("tailors_shop")
~ deactivate("streets")
~ teleport("plant")
->END


=== toStreet ===
~ activate("streets")
~ deactivate("tailors_shop")
~ teleport("pinkDoor")
~activate("cam_dark_street")
~deactivate("cam_in_tailor")
->END

=== streetToPolice ===
~ activate("police_office")
~ deactivate("streets")
~ teleport("DarkBooks7")
{-OfficerHasBigHat: 
~OfficerBigHat()
}
->END

=== policeToStreet ===
~ activate("streets")
~ deactivate("police_office")
~ teleport("streetToPolice")
->END


=== StreetToPort ===
~ activate("port")
~ deactivate("streets")
~ teleport("FaintHouseAtPort")
->END

=== PortToStreet ===
~ activate("streets")
~ deactivate("port")
~ teleport("dark_house_9 (2)")
->END



=== PortToBoat ===
{
- KEY == true: -> openBoat
- else: -> lockedBoat
}


=openBoat
~ activate("boat_Interior")
~ deactivate("port")
~ teleport("ControlBoard")
->END


=lockedBoat
You need the boat key. Probably in some government employee's hand.
->END

=== boatToPort ===
~ activate("port")
~ deactivate("boat_Interior")
~ teleport("doorToBoat")
->END

=== sail ===
~deactivate("BlackScreen")
~ activate("frat_building")
~ deactivate("boat_Interior")
~ teleport("fratToBoat")
->UnionBuildingOnStart
->END


=== streetToApartment ===
~ activate("apt_interior")
~ deactivate("streets")
~ teleport("Teleport")
->END

=== apartmentToStreet ===
~ activate("streets")
~ deactivate("apt_interior")
~ teleport("StreetToApt")
->END

=== aptHallToBedroom ===
~ activate("bedroom")
~ deactivate("apt_interior")
~ teleport("OpenedBook")
->BedroomIntro

=== bedroomToHall ===
~ activate("apt_interior")
~ deactivate("bedroom")
~ teleport("toRoom")
->END


=== BoatToFratHouse ===


->END



=== FratHouseToSecretChamber ===

->END


=== SecretChamberToUpperCity===

->END





=== portalA ===
~ teleport("orangePortal")
->END

=== enterPortal ===
Entering the portal!
~ activate("portal")
~ deactivate("frat_building")
~ teleport("portalToFrat")
->END

=== AtBottomOfPortal ===
(placeholder)Some monologue
->END

===FratToUpper===
~ activate("upper_streets")
~ deactivate("frat_building")
~ teleport("Bistro")

->END

->END






x