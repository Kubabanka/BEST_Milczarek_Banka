from scapy.all import *
import base64

received_data = bytes(0)

def finish_sniffing(pkt):
	global received_data
	return len(received_data) > 19000


def handle_packet(msg):

	global received_data	

	if UDP in msg and msg[UDP].dport == 5062 and msg[UDP].chksum == 48127:	
		print("Data Chunk Acquired")
		msg[UDP].payload = RTP(msg[Raw].load)
		
		try:
			base64.b32decode(msg[RTP].load)
			received_data += msg[RTP].load
			print(len(received_data))
		except Exception:
			print("Wrong packet")
		
			
	

sniff(filter='dst host 192.168.0.106 and src host 192.168.0.102 and dst port 5062', prn=handle_packet, store=0, stop_filter=finish_sniffing)

myFile = open('data.txt', 'w')
myFile.write(base64.b32decode(received_data).decode('utf-8'))
myFile.close()

print("Koniec")


