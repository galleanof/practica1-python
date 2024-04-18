#! /usr/bin/python

# 1ra Practica Laboratorio 
# Teoria de Grafos
# Consigna: Implementar los siguientes metodos

import sys
import os

def eliminar_espacio_tripleta(tripleta):
    
    tripla_lista = list(tripleta)
    
    indice_espacio = tripla_lista.index(' ')
    
    tripla_lista.pop(indice_espacio)
    
    nueva_tripleta = tuple(tripla_lista)
    return nueva_tripleta

def lee_grafo_stdin(grafo):
    """
    Recibe como argumento un grafo representado como una lista de tipo:
    Ejemplo Entrada: 
       ['3', 'A', 'B', 'C', 'A B', 'B C', 'C B']
    
    donde el 1er elemento se corresponde a la cantidad de vertices,
    y por ultimo las aristas existentes.

    Ejemplo retorno: 
        (['A','B','C'],[('A','B'),('B','C'),('C','B')])
    """
    primeraLista = []
    segundaLista = []
    tuplas = ()
    cantV = int (grafo[0])

    for i in range(1 , cantV+1):
        primeraLista.append(grafo[i]) 

    for i in range (cantV+1 , len(grafo)):
        segundaLista.append(eliminar_espacio_tripleta(tuple(grafo[i])))
    
    tuplaFinal = (primeraLista, segundaLista)
    return tuplaFinal



def lee_grafo_archivo(file_path):
    '''
    Lee un grafo desde un archivo y devuelve su representacion como lista.
    Ejemplo Entrada: 
        3
        A
        B
        C
        A B
        B C
        C B
    Ejemplo retorno: 
        (['A','B','C'],[('A','B'),('B','C'),('C','B')])
    '''
    data_input = []

    with open(file_path, 'r') as file:
        for line in file:
            data_input.append(line.strip())

    return lee_grafo_stdin(data_input)

def imprime_grafo_lista(grafo):
    '''
    Muestra por pantalla un grafo. El argumento esta en formato de lista.
    '''
    pass

#################### FIN EJERCICIO PRACTICA ####################
def lee_entrada_1():
    '''
    Lee un grafo desde entrada estandar y devuelve su representacion como lista.
    Ejemplo Entrada: 
        3
        A
        B
        C
        A B
        B C
        C B
    Ejemplo retorno: 
        ['3', 'A', 'B', 'C', 'A B', 'B C', 'C B']
    '''
    data_input = []
    
    for line in sys.stdin:
        if line == '\n':
            break
        else:
            # Con strip() eliminamos los '\n' del final de c/line
            data_input.append(line.strip())
            
    return data_input

    
def lee_entrada_2():
    count = 0
    try:
        while True:
            line = input()
            count = count + 1
            print ('Linea: [{0}]').format(line)
    except EOFError:
        pass
    
    print ('leidas {0} lineas').format(count)

def main():
   # grafo = lee_entrada_1()
    file_path = "/home/franco/Documents/archivoPython.txt"

    with open(file_path, 'r') as file:
            file_content = file.read()

    print(file_content)
    print(lee_grafo_archivo(file_path))

    print(lee_grafo_stdin(['4', 'A', 'B', 'C','D', 'A B', 'B C', 'C D','D A']))
if __name__=='__main__':
    main()
