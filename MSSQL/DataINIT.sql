Insert into clients values
('5862288995','Spedfor','Dworska 13/352','02-147','Poland',65000)

Insert into clients values
('53322887886','GlobeAir','Kwaitowa 52','04-647','Poland',95000)

Insert into clients values
('x','WorldCargo','Grove street 16','TX26513','USA',150000)

Insert into Service_Providers values
('9162200173','AskoAC','Bitumiczna 60','14-633','Poland')

Insert into Service_Providers values
('9188922373','UTBytniewski','Polna 40','24-633','Poland')

Insert into Service_Providers values
('8080144373','MagMaster','Polna 50','24-633','Poland')

Insert into Forwarders values
('Tomasz','Bal','TBA', convert(VARBINARY(max), 'pass1'), convert(VARBINARY(max), 'pass1'))

Insert into Forwarders values
('Anastazja','Wolska','AWO', convert(VARBINARY(max), 'pass2'), convert(VARBINARY(max), 'pass2'))

Insert into Forwarders values
('Kamil','Kotarski','KKO', convert(VARBINARY(max), 'pass3'), convert(VARBINARY(max), 'pass3'))

Insert into Cases values
('SI/1/TBA/8/2022',10000,'SI',1)

Insert into Invoices values
(23,'2022-08-01','2022-08-01',12300,1,1)

Insert into Service values
('Transport drogowy',10000,23,1)

Insert into Costs values
(10000,23,'Transport drogowy',1,1)

