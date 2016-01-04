from socket import*
from select import*
import sys
import json
import thread
from time import ctime

# Host = '143.248.36.223'
Host = '127.0.0.1'
Port = 2345
Bufsize = 1024
ADDR = (Host,Port)


player_pair_list = []

class client_sock:
    def __init__(self):
        self.first = None
        self.second = None
        self.roomnumber = 0
        self.firstload = False
        self.secondload = False
    def set_1P(self, client_socket1, num):
        self.first = client_socket1
        self.roomnumber = num
    def set_2P(self,client_socket2):
        self.second = client_socket2
    def set_load_1P(self):
        self.firstload = True
    def set_load_2P(self):
        self.secondload = True


def ClientThread(clientsock,addr,room_num,net_order):
    while True:
        data = clientsock.recv(Bufsize)
        try:
            client_socklist = player_pair_list[room_num]
            if checktype(data) == "Move":
                if net_order == 0:
                    client_socklist.second.send(data)
                else:
                    client_socklist.first.send(data)
            elif checktype(data) == "Load":
                if net_order == 0:
                    client_socklist.first.set_load_1P()
                else:
                    client_socklist.second.set.load_2P()
                if client_socklist.firstload == True and client_socklist.secondload == True:
                    client_socklist.first.send(_Serializer("Start",'',-1))
                    client_socklist.second.send(_Serializer("Start",'',-1))
            print "Data send to opponent successfully"
        except error:
            print "Data could not be sent"
    clientsock.close()
    print repr(addr) + ' ' + "end connection1\n"

def checktype(msg):
    js  = json.loads(msg)
    _Type = js['Type']
    if (_Type == 'MoveStart') or (_Type == 'MoveEnd'):
        return "Move"
    elif (_Type == 'Load'):
        return 'Load'


def _Serializer(__type,arg, netorder):
    return json.dumps (({"Type":__type,"Arguments":arg,"NetworkOrder":netorder}),indent=4,separators=(",",":"))

if __name__ == '__main__':
    serverSocket = socket(AF_INET,SOCK_STREAM)
    serverSocket.setsockopt(SOL_SOCKET,SO_REUSEADDR,1)
    serverSocket.bind(ADDR)

    serverSocket.listen(5)

    print("Server Start")
    room_num = 1
    while 1:
        print 'waiting for connection'
        client_list = client_sock()
        for i in range(2):
            clientsock, addr = serverSocket.accept()
            if i==0:
                room_num += 1
                client_list.set_1P(clientsock,room_num)
                net_order = 1
                clientsock.send(_Serializer("PlayerOrder",[net_order],-1))
            else:
                client_list.set_2P(clientsock)
                player_pair_list.append(client_list)
                net_order = 2
                clientsock.send(_Serializer("PlayerOrder",[net_order],-1))
                client_list.first.send(_Serializer("Connect",[],-1))
                client_list.second.send(_Serializer("Connect",[],-1))
            print 'connected from' , addr
            thread.start_new_thread(ClientThread, (clientsock, addr,room_num,net_order))
