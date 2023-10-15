import { RouterModule } from "@angular/router";
import { Checkout } from "../pages/checkout.component";
import { ShopPage } from "../pages/shopPage.component";
import { LoginPage } from "../pages/loginPage.component";
import { AuthActivator } from "../services/authActivator.services";

const routes = [
    { path: "", component: ShopPage },
    { path: "checkout", component: Checkout, canActivate: [AuthActivator] },
    { path: "login", component: LoginPage },
    { path: "**", redirectTo: "/" },
];

const router = RouterModule.forRoot(routes);
export default router;