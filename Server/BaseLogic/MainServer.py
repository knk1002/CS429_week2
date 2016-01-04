from socket import*
from select import*
import sys
import json
import thread
from time import ctime

Host = '143.248.36.223'
Port = 2345
Bufsize = 1024
ADDR = (Host,Port)


player_pair_list = []

class client_sock:
    def set_1P(self, client_socket1, num):
        self.first = client_socket1
        self.roomnumber = num
    def add_player_pair(self):
        pass
    def set_2P(self,client_socket2,num):
        self.second = client_socket2
        self.add_player_pair()


def ClientThread(clientsock,addr):
    _data = ''
    while True:
        data = clientsock.recv(Bufsize)
        if _data == data:
            break
        else:
            _data = data
        print repr(addr) + ' ' + repr(data)
    clientsock.close()
    print repr(addr) + ' ' + "end connection1\n"

def _JSONread(datum):
    js  = json.loads(datum)
    _Type = js['Type']
    _NetworkOrder = js['NetworkOrder']
    _Arguments = js['Arguments']




if __name__ == '__main__':
    serverSocket = socket(AF_INET,SOCK_STREAM)
    serverSocket.setsockopt(SOL_SOCKET,SO_REUSEADDR,1)
    serverSocket.bind(ADDR)

    serverSocket.listen(5)

    print("Server Start")
    room_num = 0
    while 1:
        print 'waiting for connection'
        for i in range(2):
            client_list = client_sock()
            clientsock, addr = serverSocket.accept()
            print clientsock
            if i==0:
                room_num += 1
                client_list.set_1P(clientsock,room_num)
                print "You are Player 1"
            else:
                client_list.set_2P(clientsock,room_num)
                player_pair_list.append(client_list)
                print "You are Player 2"
            print 'connected from' , addr
            thread.start_new_thread(ClientThread, (clientsock, addr))
            if i == 1:
                print (player_pair_list[0])
