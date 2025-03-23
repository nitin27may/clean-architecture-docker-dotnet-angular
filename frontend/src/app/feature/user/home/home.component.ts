import {
    Component,
    Inject,
    PLATFORM_ID,
    ViewEncapsulation,
} from '@angular/core';
import { environment } from "../../../../environments/environment";

@Component({
    selector: 'app-home',
    imports: [],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss',
    encapsulation: ViewEncapsulation.None
})
export class HomeComponent {
    name = 'Contacts';
    angular = environment.angular;
    bootstrap = environment.bootstrap;
    dotnet = environment.dotnet;
    mssql = environment.mssql;

    constructor(@Inject(PLATFORM_ID) private platformId: object) {}
}
