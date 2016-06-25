import { Component } from '@angular/core';
import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from '@angular/router-deprecated';
import {ResultsComponent} from "../recipe-results/results.component";

@Component({
    selector: 'voice-input',
    templateUrl: './app/voice-input/input.component.html',
    providers: [
        ROUTER_PROVIDERS
    ]
})
export class InputComponent { }