Unfortunately code first migrations messed up and I couldn't get it to cleanly update the quotes data structure.

So for this commit you will have to drop the databse:
	(View -> Sql Server Object Explorer->SQL Server->localhost\SQLEXPRESS->Databases->SGFastFlyers1 -> Right click delete (or something like that))

 Then you will have to quit Visual Studio, Relaunch, hit Rebuild Solution and then in the Package Manager Console (Tools->NuGet Package Manager->Package Manager Console) type the following:
	Update-Database

	And it should set the database up and pre-populate it with 8 bogus orders.

	Hopefully this will be the end of those validation errors..

Chris.