from socket import*
from select import*
import sys
import json
import thread
import socket
import time
from time import ctime

Host = '143.248.225.132'
#Host = '127.0.0.1'
Port = 2345
Bufsize = 1024
ADDR = (Host,Port)
now_count = 0


player_pair_list = []

class client_sock:
    def __init__(self):
        self.first = None
        self.second = None
        self.roomnumber = 0
        self.firstload = False
        self.secondload = False
        self.firstqueue = []
        self.secondqueue = []
    def set_1P(self, client_socket1, num):
        self.first = client_socket1
        self.roomnumber = num
    def set_2P(self,client_socket2):
        self.second = client_socket2
    def set_load_1P(self):
        self.firstload = True
    def set_load_2P(self):
        self.secondload = True


def recv_json(sock,room_num,net_order):
    i =0
    try:
        while True:
            data = sock.recv(Bufsize)
            client_socklist = player_pair_list[room_num]
            if checktype(data) == "Move":
                if net_order == 1:
                    print checktype(data)
                    client_socklist.firstqueue.append([data,client_socklist.second])
                else:
                    print checktype(data)
                    client_socklist.secondqueue.append([data,client_socklist.first])
            elif checktype(data) == "Load":
                if net_order == 1:
                    client_socklist.set_load_1P()
                    while client_socklist.secondload == False:
                        time.sleep(0)
                else:
                    client_socklist.set_load_2P()
                if client_socklist.firstload == True and client_socklist.secondload == True:
                    client_socklist.first.send(_Serializer(3,'',-1))
                    client_socklist.second.send(_Serializer(3,'',-1))
            print "Data send to opponent successfully"
            time.sleep(0)
    except socket.error, e:
        pass

def send_json(sock,room_num,net_order):
    client_socklist = player_pair_list[room_num]
    i = 0
    while True:
        if(net_order == 2):
            if(not client_socklist.secondqueue == []):
                client_socklist.secondqueue[0][1].send(client_socklist.secondqueue[0][0])
                del client_socklist.secondqueue[0]

        else:
            if(not client_socklist.firstqueue == []):
                client_socklist.firstqueue[0][1].send(client_socklist.firstqueue[0][0])
                del client_socklist.firstqueue[0]
        time.sleep(0)



def ClientThread(clientsock,addr,room_num,net_order):
    thread.start_new_thread(recv_json,(clientsock,room_num,net_order))
    while player_pair_list[room_num].firstload == False or player_pair_list[room_num].secondload == False:
        time.sleep(0)
    thread.start_new_thread(send_json,(clientsock,room_num,net_order))


def checktype(msg):
    js  = json.loads(msg)
    _Type = js['Type']
    if (_Type == 4) or (_Type == 5):
        return "Move"
    elif (_Type == 2):
        return 'Load'


def _Serializer(__type,arg, netorder):
    return json.dumps (({"Type":__type,"Arguments":arg,"NetworkOrder":netorder}),indent=4,separators=(",",":"))

if __name__ == '__main__':
    serverSocket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    serverSocket.setsockopt(SOL_SOCKET,SO_REUSEADDR,1)
    serverSocket.bind(ADDR)

    serverSocket.listen(5)

    print("Server Start")
    room_num = -1
    while 1:
        print 'waiting for connection'
        clientsock, addr = serverSocket.accept()
        if(len(player_pair_list) == 0 or not(player_pair_list[room_num].second == None)):
            client_list = client_sock()
            player_pair_list.append(client_list)
            room_num += 1
            net_order = 1
        print 'connected from' , addr
        if(net_order == 1):
            player_pair_list[room_num].first = clientsock
        else:
            player_pair_list[room_num].second = clientsock
            client_socklist = player_pair_list[room_num]
            if(not(player_pair_list[room_num].first == None) and not(player_pair_list[room_num].second == None)):
                client_socklist.first.send(_Serializer(1,[],-1))
                client_socklist.second.send(_Serializer(1,[],-1))

        clientsock.send(_Serializer("PlayerOrder",[net_order],-1))
        thread.start_new_thread(ClientThread, (clientsock, addr,room_num,net_order))
        net_order += 1

