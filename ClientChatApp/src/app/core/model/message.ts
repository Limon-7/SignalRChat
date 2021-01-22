export interface Message {
    id: number;
    senderId: number;
    senderLastName: string;
    recipientId: number;
    recipientLastName: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
