/// <reference path="../../typings/tsd.d.ts" />
/// <amd-dependency path="common/broadcast.service" />

import LazyLoading = require('common/lazyLoading.module');
import BroadcastService = require('common/broadcast.service');

module Spine {

	export interface ISpineScope extends ng.IScope  {
		id: string;
        width: string;
		height: string;
        file: string;
        anim: string;
        repeat: boolean;
        scale: number;
	}
    
	export class SpineController {	
		broadcastService: BroadcastService;
		timeout: ng.ITimeoutService;
		destroyBroadcastListener: Function;
	    scope: ISpineScope;
        stage: PIXI.Container;
        renderer: any;
        spine: any;
        
		static $inject = [
			'$scope',
			'BroadcastService',
			'$timeout'	
		];
		
		constructor(scope: ISpineScope, broadcastService: BroadcastService, timeout: ng.ITimeoutService) {
			this.broadcastService = broadcastService;
			this.timeout = timeout;
            this.scope = scope;
            
            this.init();
					
			this.destroyBroadcastListener = this.broadcastService.listen('spine-anim', (event: any, model: any) => {
                if (model.id === this.scope.id) {
                    this.spine.state.setAnimationByName(0, model.anim, model.repeat);
                }
			});
	
			scope.$on('$destroy', () => {
				this.destroyBroadcastListener();
			});		            
		}            
        
        init() {
            let width = parseInt(this.scope.width, 10);
            let height = parseInt(this.scope.height, 10);
            
            this.renderer = PIXI.autoDetectRenderer(width, height, {transparent: true});
            $('#' + this.scope.id).append(this.renderer.view);

            this.stage = new PIXI.Container();

            // load spine data
            let self = this;            
            PIXI.loader
                .add(this.scope.id, 'dist/spine/' + this.scope.file)
                .load((loader, res) => {
                   self.onAssetsLoaded(loader, res); 
                });

            this.stage.interactive = true;

            requestAnimationFrame(() => { self.animate(); });
        }
        
        onAssetsLoaded(loader, res) {
            // create a spine boy
            this.spine = new (<any>PIXI).spine.Spine(res[this.scope.id].spineData);

            // set the position
            this.spine.position.x = this.renderer.width / 2;
            this.spine.position.y = this.renderer.height;

            this.spine.scale.set(this.scope.scale);
            
            // play animation
            this.spine.state.setAnimationByName(0, this.scope.anim, this.scope.repeat);

            this.stage.addChild(this.spine);
        }            
        
        animate() {
            let self = this;            
            requestAnimationFrame(() => { self.animate(); });
            self.renderer.render(this.stage);
        }        
	}
	
	function spine(): ng.IDirective {
		return {
			restrict: 'E',
            scope: {
                id: '@',
                width: '@',
                height: '@',
                file: '@',
                anim: '@',
                repeat: '=',
                scale: '@'
            },
			controller: SpineController,
			controllerAs: 'spineCtrl',
			template: '<div class="spine">' +
                      '</div>',
			replace: true
		};
	}
	
	LazyLoading.Application.registerDirective('spine', spine);
}


export = Spine;