USE WEBApplication;
GO

CREATE  TABLE [dbo].[Atores] (
  AtoresID INT NOT NULL IDENTITY(1,1),
  Nome VARCHAR(100) NOT NULL,
  DataNascimento DATE NOT NULL,
  PRIMARY KEY (AtoresID),
  INDEX Nome_idx (Nome ASC)
);
GO

CREATE  TABLE [dbo].[Filmes] (
  FilmeID INT NOT NULL IDENTITY(1,1),
  Titulo VARCHAR(100) NOT NULL,
  AnoLancamento INT NULL,
  Duracao INT NULL,
  PRIMARY KEY (FilmeID),
  INDEX Titulo_idx (Titulo ASC) 
);
GO

CREATE  TABLE [dbo].[Generos] (
  GeneroID INT NOT NULL IDENTITY(1,1),
  Descricao VARCHAR(100) NOT NULL,
  PRIMARY KEY (GeneroID),
  INDEX Genero_idx (Descricao ASC) 
);
GO

CREATE  TABLE [dbo].[Papeis] (
  FilmeID INT NOT NULL,
  AtorID INT NOT NULL,
  NomePersonagem VARCHAR(100) NOT NULL,
  PRIMARY KEY (FilmeID, AtorID),
  INDEX FK_FilmeID_idx (FilmeID ASC),
  INDEX FK_AtorID_idx (AtorID ASC),
  INDEX Personagem_idx (NomePersonagem ASC),
  CONSTRAINT FK_FilmeID_Papeis
    FOREIGN KEY (FilmeID)
    REFERENCES [dbo].[Filmes] (FilmeID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_AtorID_Papeis
    FOREIGN KEY (AtorID)
    REFERENCES [dbo].[Atores] (AtoresID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO

CREATE  TABLE [dbo].[Studios] (
  StudioID INT NOT NULL IDENTITY(1,1),
  Nome VARCHAR(100) NOT NULL,
  Cidade VARCHAR(100) NULL,
  PRIMARY KEY (StudioID),
  INDEX Nome_idx (Nome ASC)
);
GO

CREATE  TABLE [dbo].[Colaboracoes] (
  StudioID INT NOT NULL ,
  FilmeID INT NOT NULL ,
  PRIMARY KEY (StudioID, FilmeID) ,
  INDEX FK_StudioID_idx (StudioID ASC) ,
  INDEX FK_FilmeID_idx (FilmeID ASC) ,
  CONSTRAINT FK_StudioID_Colaboracoes
    FOREIGN KEY (StudioID)
    REFERENCES [dbo].[Studios] (StudioID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_FilmeID_Colaboracoes
    FOREIGN KEY (FilmeID)
    REFERENCES [dbo].[Filmes] (FilmeID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO

CREATE  TABLE [dbo].[GeneroFilme] (
  GeneroID INT NOT NULL,
  FilmeID INT NOT NULL,
  PRIMARY KEY (GeneroID, FilmeID),
  INDEX FK_GeneroID_idx (GeneroID ASC),
  INDEX FK_FilmeID_idx (FilmeID ASC),
  CONSTRAINT FK_GeneroID_GeneroFilme
    FOREIGN KEY (GeneroID)
    REFERENCES [dbo].[Generos] (GeneroID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_FilmeID_GeneroFilme
    FOREIGN KEY (FilmeID)
    REFERENCES [dbo].[Filmes] (FilmeID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO

CREATE  TABLE [dbo].[Reviews] (
  ReviewID INT NOT NULL IDENTITY(1,1),
  FilmeID INT NOT NULL,
  Nota INT NOT NULL,
  Resenha TEXT NULL,
  PRIMARY KEY (ReviewID),
  INDEX FK_FilmeID_idx (FilmeID ASC),
  CONSTRAINT FK_FilmeID_Reviews
    FOREIGN KEY (FilmeID)
    REFERENCES [dbo].[Filmes] (FilmeID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO