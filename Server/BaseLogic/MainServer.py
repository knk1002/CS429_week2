from socket import*
from select import*
import sys
from time import ctime

Host = ''
Port = 2345
Bufsize = 1024
ADDR = (Host,Port)

serverSocket = socket(AF_INET,SOCK_STREAM)

serverSocket.bind(ADDR)

serverSocket.listen(10)
connection_list = [serverSocket]

print("Server Start")

while connection_list:
    try:
        print("wait connection")

        read_socket, write_socket, error_socket = select(connection_list,[],[],10)

        for sock in read_socket:
            if sock == serverSocket:
                clientSocket, addr_info = serverSocket.accept();
                connection_list.append(clientSocket)
                print("Client Connect" % (ctime(), addr_info[0]))

                for socket_in_list in connection_list:
                    if socket_in_list != serverSocket and socket_in_list != sock:
                        try:
                            socket_in_list.send("server respose")
                        except Exception as e:
                            socket_in_list.close()
                            connection_list.remove(socket_in_list)
            else:
                data = sock.recv(Bufsize)
                if data:
                    print data
                else:
                    connection_list.remove(sock)
                    sock.close()
                    print("fail to continuous connection")
    except KeyboardInterrupt:
        serverSocket.close()
        sys.exit()