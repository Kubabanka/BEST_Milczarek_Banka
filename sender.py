from scapy.all import *
from netfilterqueue import NetfilterQueue

i = 0
delayed_pck: object = IP()

index = 0
bytes_to_send: bytes
offset = random.randint(-50,50)

os.system('iptables -I OUTPUT -d 192.168.0.0/24 -j NFQUEUE --queue-num 1')

opened_file = open("/home/jbanka/Desktop/BEST/Sofokles - Antygona.txt", "r")
txt = opened_file.read()
bytes_to_send = base64.b32encode(txt.encode('utf-8'))


def send_delayed_pck():
    global delayed_pck
    time.sleep(1)
    delayed_pck.accept()


def handle_packet(packet):
    pkt = IP(packet.get_payload())

    global i
    global delayed_pck
    global bytes_to_send
    global index
    global offset

    if UDP in pkt and pkt[UDP].dport == 5062:
        pkt[UDP].payload = RTP(pkt[Raw].load)

    if pkt.haslayer(RTP):
        i += 1

    if i == 499 + offset:
        i = 0
        offset = random.randint(-50,50)
        
        load_len = len(pkt[RTP].load)
        pkt[RTP].load = bytes_to_send[index: index + load_len]
        index += load_len
        if index >= len(bytes_to_send):
            index = 0

        pkt[UDP].chksum = int.from_bytes(b'\xbb\xff', byteorder='big')

        del pkt[IP].chksum
        packet.set_payload(bytes(pkt))
        delayed_pck = packet
        t = threading.Thread(name='Delay', target=send_delayed_pck)
        t.setDaemon(True)
        t.start()

    else:
        packet.accept()




nfqueue = NetfilterQueue()
nfqueue.bind(1, handle_packet)
try:
    nfqueue.run()
except KeyboardInterrupt:
    os.system('iptables --flush')

nfqueue.unbind()
