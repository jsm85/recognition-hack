import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { DataService } from './services/data.service';

@Component({
    selector: 'luigi-app',
    templateUrl: './app/app.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        DataService
    ]
})

export class AppComponent {
    title = 'Luigi';
}