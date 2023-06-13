##
## EPITECH PROJECT, 2022
## HomeTodo
## File description:
## Makefile
##

# C SHARP SPECIAL MAKEFILE
BIN		=	TodoAPI
CMD 	=	dotnet build
SRC		=	$(wildcard *.cs)
OPT		=	--verbosity quiet

all: $(BIN)

$(BIN): $(SRC)
	$(CMD) $(OPT) -o $(BIN)

clean:
	rm -rf $(BIN)

fclean: clean

re: clean all

exe: re
	$(BIN)/$(BIN)
