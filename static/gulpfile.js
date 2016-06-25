/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp");

var webroot = "./wwwroot/";

gulp.task("scripts", () => {
    gulp.src([
            'es6-shim/es6-shim.min.js',
            'systemjs/dist/system-polyfills.js',
            'systemjs/dist/system.src.js',
            'reflect-metadata/Reflect.js',
            'core-js/client/core.js',
            'core-js/client/shim.min.js',
            'rxjs/**',
            'zone.js/dist/**',
            '@angular/**'
    ], {
        cwd: "node_modules/**"
    })
        .pipe(gulp.dest("./wwwroot/libs"));
});

