
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre NVARCHAR(100) NOT NULL UNIQUE 
);

CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre NVARCHAR(100) NOT NULL UNIQUE 
);

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    Password NVARCHAR(256) NOT NULL,
	RolId INT
	FOREIGN KEY (RolId) REFERENCES Roles(Id) 
);

CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Precio DECIMAL(18, 2) NOT NULL,
    UsuarioId INT NOT NULL, 
    CategoriaId INT NOT NULL, 
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) , 
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id) 
);

