

export class Order {
    orderId?: number;
    orderDate?: Date = new Date();
    orderNumber?: string;
    items: OrderItem[] = [];
}

export class OrderItem {
    id?: number;
    quantity: number = 0;
    unitPrice: number = 0.0;
    productId?: number;
    productCategory?: string;
    productSize?: string;
    productTitle?: string;
    productArtist?: string;
    productArtId: string = "";
}