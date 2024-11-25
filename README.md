A játék kliens-szerver architektúrát használ, ahol a játékosok socket kommunikáción keresztül csatlakoznak egymáshoz.

Játéktábla:
A játék egy 10x10-es rácsot használ.
Mindkét játékos rendelkezik saját játéktáblával, amelyre hajókat helyezhet.


Hajók:
Minden játékos 5 hajót helyezhet el a tábláján:
1 darab 5 mező hosszú (Aircraft Carrier)
1 darab 4 mező hosszú (Battleship)
2 darab 3 mező hosszú (Submarine és Cruiser)
1 darab 2 mező hosszú (Destroyer)


Játék menete:
A játék kezdetén mindkét játékos elhelyezi a hajóit a tábláján. A hajók vízszintesen vagy függőlegesen helyezkedhetnek el, és nem érinthetik egymást sem mezővel, sem sarokkal.
A játékosok felváltva adnak le lövéseket az ellenfél táblájára. A lövés koordinátáit a kliens küldi el a szervernek, aki továbbítja az információt a másik játékosnak.
Ha a lövés találat, a szerver visszajelzést küld a találat sikerességéről, és az adott mező jelölése megváltozik.
A játék akkor ér véget, amikor valamelyik játékos összes hajóját eltalálták.

Technikai követelmények:
A kliens és a szerver kommunikációját TCP socketeken keresztül kell megvalósítani.
Az alkalmazásnak képesnek kell lennie kezelni a hálózati hibákat, például elvesztett kapcsolatot.

Plussz pont:
Az interfész legyen felhasználóbarát, a hajók elhelyezését egyszerű drag-and-drop módszerrel lehessen kezelni.



Feladatkiadás:
Az elkészült projektet a forráskóddal együtt kell benyújtani.
A projekt dokumentációjában foglald össze az alkalmazott technikai megoldásokat, valamint a játék működését.
