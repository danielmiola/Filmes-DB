USE WEBApplication;
GO

INSERT INTO [dbo].[Atores] (Nome, DataNascimento) 
	VALUES ('Elijah Wood', '1981-01-28'),
		   ('Orlando Bloom', '1977-01-13'),
		   ('Viggo Mortensen', '1958-10-20'),
		   ('Ian McKellen', '1939-05-29');
GO

INSERT INTO [dbo].[Filmes] (Titulo, AnoLancamento, Duracao)
	VALUES ('The Lord of The Rings: The Fellowship of The Ring', 2001, 178),
		   ('The Lord of The Rings: The Two Towers', 2002, 179),
		   ('The Lord of The Rings: The Return of the King', 2003, 201);
GO

INSERT INTO [dbo].[Generos] (Descricao)
	VALUES ('Adventure'),
		   ('Fantasy'),
		   ('Drama'),
		   ('Crime'),
		   ('Action');
GO

INSERT INTO [dbo].[Studios] (Nome, Cidade)
	VALUES ('Universal Studios', 'Los Angeles'),
	       ('LucasFilm', 'San Francisco'),
		   ('New Line Cinema', 'New York');
GO