-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2022-07-30 16:31:48.694

-- tables
-- Table: Cases
CREATE TABLE Cases (
    ID int  NOT NULL IDENTITY(1,1),
    Name varchar(25)  NOT NULL,
    Amount int  NOT NULL,
    Relation varchar(2)  NOT NULL,
    Forwarders_ID int  NOT NULL,
    CONSTRAINT Cases_pk PRIMARY KEY  (ID)
);

-- Table: Clients
CREATE TABLE Clients (
    ID int  NOT NULL IDENTITY(1,1),
    TAX varchar(20)  NOT NULL,
    Name varchar(50)  NOT NULL,
    Street varchar(80)  NOT NULL,
    ZIP varchar(10)  NOT NULL,
    Coutry varchar(54)  NOT NULL,
    Credit decimal(15,2)  NOT NULL,
    CONSTRAINT Clients_pk PRIMARY KEY  (ID)
);

-- Table: Costs
CREATE TABLE Costs (
    ID int  NOT NULL IDENTITY(1,1),
    Amount decimal(15,2)  NOT NULL,
    TAX int  NOT NULL,
    Name varchar(50)  NOT NULL,
    Cases_ID int  NOT NULL,
    Service_Providers_ID int  NOT NULL,
    CONSTRAINT Costs_pk PRIMARY KEY  (ID)
);

-- Table: Forwarders
CREATE TABLE Forwarders (
    ID int  NOT NULL IDENTITY(1,1),
    Name varchar(30)  NOT NULL,
    Surname varchar(40)  NOT NULL,
    Prefix varchar(5)  NOT NULL,
    CONSTRAINT Forwarders_pk PRIMARY KEY  (ID)
);

-- Table: Invoices
CREATE TABLE Invoices (
    ID int  NOT NULL IDENTITY(1,1),
    TAX int  NOT NULL,
    Issue_Date date  NOT NULL,
    Service_Date date  NOT NULL,
    Amount decimal(15,2)  NOT NULL,
    Cases_ID int  NOT NULL,
    Clients_ID int  NOT NULL,
    CONSTRAINT Invoices_pk PRIMARY KEY  (ID)
);

-- Table: Service
CREATE TABLE Service (
    ID int  NOT NULL IDENTITY(1,1),
    Name varchar(30)  NOT NULL,
    Amonut decimal(12,2)  NOT NULL,
    TAX int  NOT NULL,
    Invoices_ID int  NOT NULL,
    CONSTRAINT Service_pk PRIMARY KEY  (ID)
);

-- Table: Service_Providers
CREATE TABLE Service_Providers (
    ID int  NOT NULL IDENTITY(1,1),
    TAX varchar(20)  NOT NULL,
    Name varchar(50)  NOT NULL,
    Street varchar(80)  NOT NULL,
    ZIP varchar(10)  NOT NULL,
    Coutry varchar(54)  NOT NULL,
    CONSTRAINT Service_Providers_pk PRIMARY KEY  (ID)
);

-- foreign keys
-- Reference: Cases_Forwarders (table: Cases)
ALTER TABLE Cases ADD CONSTRAINT Cases_Forwarders
    FOREIGN KEY (Forwarders_ID)
    REFERENCES Forwarders (ID);

-- Reference: Costs_Cases (table: Costs)
ALTER TABLE Costs ADD CONSTRAINT Costs_Cases
    FOREIGN KEY (Cases_ID)
    REFERENCES Cases (ID);

-- Reference: Costs_Service_Providers (table: Costs)
ALTER TABLE Costs ADD CONSTRAINT Costs_Service_Providers
    FOREIGN KEY (Service_Providers_ID)
    REFERENCES Service_Providers (ID);

-- Reference: Invoices_Cases (table: Invoices)
ALTER TABLE Invoices ADD CONSTRAINT Invoices_Cases
    FOREIGN KEY (Cases_ID)
    REFERENCES Cases (ID);

-- Reference: Invoices_Clients (table: Invoices)
ALTER TABLE Invoices ADD CONSTRAINT Invoices_Clients
    FOREIGN KEY (Clients_ID)
    REFERENCES Clients (ID);

-- Reference: Service_Invoices (table: Service)
ALTER TABLE Service ADD CONSTRAINT Service_Invoices
    FOREIGN KEY (Invoices_ID)
    REFERENCES Invoices (ID);

-- End of file.

