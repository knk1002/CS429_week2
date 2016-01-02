from socket import*
from select import*
import sys
import json
import thread
from time import ctime

Host = ''
Port = 2345
Bufsize = 1024
ADDR = (Host,Port)

def ClientThread(clientsock,addr):
    while True:
        data = clientsock.recv(Bufsize)
        print repr(addr) + ' ' + repr(data)
    clientsock.close()
    print repr(addr) + ' ' + "end connection"

def readConfig(name):
    f = open(name,'r')
    js = json.load(f.read())
    f.close()
    return js

def JSONread(msg):
    CONFIG = {}
    CONFIG = readConfig(json.load(msg))



# def _JSON():




if __name__ == '__main__':
    serverSocket = socket(AF_INET,SOCK_STREAM)
    serverSocket.setsockopt(SOL_SOCKET,SO_REUSEADDR,1)
    serverSocket.bind(ADDR)

    serverSocket.listen(5)

    print("Server Start")

    while 1:
        print 'waiting for connection'
        clientsock, addr = serverSocket.accept()
        print 'connected from' , addr
        thread.start_new_thread(ClientThread, (clientsock, addr))

