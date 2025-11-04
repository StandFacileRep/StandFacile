
[05.12.2024]<br>
Attenzione: <br>
il file Listino.txt va aggiunto alla directory dell'eseguibile<br>
il file StandCommonSafe.cs presente nella root va copiato nella directory: StandCommonSrc

---------------------------------------------------------------------------------

Attenzione: l'aggiornamento del package gratuito NutGet dotConnect.Express.for.SQLite,
può provocare il malfunzionamento (richiesta di licenza) della dll

---------------------------------------------------------------------------------

  Versione 5.13.0
- commit su GitHub in qualità di repository pubblica

---------------------------------------------------------------------------------

Copyright (c) 2024-2025 Mauro Artuso

Questa Licenza è relativa alla Suite di Programmi "StandFacile" che comprende:<br>
“StandFacile”, “StandCucina”, “StandMonitor”, “StandOrdini”

Tutti i Programmi della Suite sono distribuiti con Licenza:<br>
GNU General Public License pubblicata dalla Free Software Foundation: https://www.gnu.org/licenses.

Questo programma è distribuito nella speranza che possa essere utile, ma SENZA ALCUNA GARANZIA; 
senza nemmeno la garanzia implicita di COMMERCIABILITÀ o IDONEITÀ PER UNO SCOPO PARTICOLARE.

---------------------------------------------------------------------------------

Descrizione breve architettura di StandFacile:

DataManager.cs è la classe di gestione dei dati ed opera sulla struct SF_Data

dBaseIntf.cs è l'interfaccia per la gestione dei 3 databases che operano sulla struct DB_Data<br>
dBaseIntf_ql.cs gestisce il database locale (su file) SQLite<br>
dBaseIntf_my.cs gestisce il database di rete MySQL<br>
dBaseIntf_pg.cs gestisce il database di rete PostGreSQL<br>

Ho scelto di utilizzare sia MySQL sia PostGreSQL perchè hanno licenze di tipo piuttosto diverso:
PostGreSQL è più permissivo e potrebbe essere distribuito direttamente con il SW, però MySQL è più diffuso.

L'installazione del db di rete è lasciata all'utente, io ho utilizzato solo le dll di connessione.

dBaseTunnel_my.cs è la classe di connessione al DB remoto ospitato dal sito web,
utilizza una tecnica di tipo http_tunneling: la chiave CIPHER_KEY di StandFacile deve corrispondere
a quella dell' "http tunnel" presente nel sito remoto e scritto in linguaggio php.

StandCommonSafe.cs è il file che contiene le chiavi crittografiche e l'indirizzo del db_web_server


