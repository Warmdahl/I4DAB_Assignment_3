# I4DAB_Assignment_3 - DAB assignment 3 - NoSQL database

Før man går i gang skal man gøre følgende:
	- Oprette en mongo database og starte den med commandoen "mongod -dbpath "din mongodb folder path""
	- Opdatere DatabaseName i appsettings.json i projektet til at have navnet på den databasen som skal anvendes
	- Oprette de 6 collections i databasen:
		o Users
		o Posts
		o Circles
		o Comments
		o Followlist
		o Blacklist
Nu er programmet klar til at kører

Første gang programmet kørers (helst i debug for at kunne bruge det med postman) skal den først side opdateres en gang for at seede alt data.
Hvis alting er sat op rigtigt så skulle alt dummy dataen gerne ses i databasen.

Til projektet er der anvendt Postman til at undersøge om de API kald som sker i programmet stemmer overens med kravene til opgaven.
Der er vedlagt en fil til projektet: "Test_SocialNetwork_DAB.postman_collection.json" 
Dette er en Postman Collection, hvis man åbner postman og importere denne fil så vil man få alle de get og post funktioner som er bedt om.
Denne collection består af disse kommandoer:
	• Man skal bruge API/Users for at få alle users 
	• Man skal bruge API/Users/{user id} for at se denne users feed 
	• Man skal bruge API/Users/{user id 1}/{user id 2} for at se user 2s wall som user 1 
	• Man skal bruge API/comment/{post id} for at se comments for en given post 
	• Man kan lave en ny post som en post under API/posts og indsætte følgende 
		o { 	
			ispublic" : "true", 
			"Circle_Id" : "5eb19f9315b7492cf4ef4cb3", 
			"Poster_Id" : "5eb162f54c9e7257443dc094", 
			"Text" : "My gun is made of silver you smuck", 
			"Image" : "null" 
		  } 
	• Man kan lave en ny comment som en post under API/comment og indsætte følgende 
		o { 	
			"PostId" : "5eb174943bf78d2074a1b7c7", 
			"text" : "But Johnny you are over there!", 
			"commenterId" : "5eb162f54c9e7257443dc094" 
		  } 
Når man bruger denne collection vil man i det første kald få alle user som er oprettet, det er her vigtigt at man anvender de ID's som de users man ser der har til de efterfølgende kald.

******************Lavet af**********************
- Alexander Winther Hoffmann 	AU566995
- Emil Leth Warmdahl  		Au576051
- Jonathan Hartvigsen Juncker 	Au618464 
- Nicolai Dreves Nielsen  	Au550273  

************************************************