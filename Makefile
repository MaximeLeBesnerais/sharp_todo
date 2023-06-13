##
## EPITECH PROJECT, 2022
## HomeTodo
## File description:
## Makefile
##

# C SHARP SPECIAL MAKEFILE
BIN		=	TodoAPI
CMD 	=	dotnet build
OPT		=	--verbosity quiet

all: $(BIN)

$(BIN):
	$(CMD) $(OPT) -o $(BIN)

clean:
	rm -rf $(BIN)

re: clean all

exe: all
	$(BIN)/$(BIN)
