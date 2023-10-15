import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Product } from "../shared/Prodcut";
import { Observable } from "rxjs";
import { Order, OrderItem } from "../shared/Order";

@Injectable()
export class Store {
    constructor(private http: HttpClient) {

    }

    public order: Order = new Order();
    public products: Product[] = [];

    loadProducts(): Observable<void> {
        return this.http.get<[]>("/api/products")
            .pipe(map(data => {
                this.products = data;
                return;
            }));
    }

    addToOrder(product: Product) {
        let item: OrderItem | undefined = undefined;

        item = this.order.items.find(o => o.productId === product.id);
        console.log(product.artId); console.log(product.category);

        if (item !== undefined) {
            item.quantity++;
        } else { 
            item = new OrderItem();
            item.productId = product.id; 
            item.unitPrice = product.price;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.productTitle = product.title;
            item.productArtist = product.artist;
            item.productArtId = product.artId;
            item.quantity = 1; 
            if (this.order !== undefined)
                this.order.items.push(item);
        }
    }
}