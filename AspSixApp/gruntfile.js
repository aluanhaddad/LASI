/// <binding AfterBuild='qunit' Clean='bower:install' ProjectOpened='watch:test, bower:install' />
// This file in the main entry point for defining grunt tasks and using grunt plugins.
// Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409

module.exports = function (grunt) {
    var scriptDir = 'Scripts/app/**/*.js';
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: 'wwwroot/lib/',
                    layout: 'byType',
                    cleanTargetDir: true
                }
            }
        },
        jslint: {
            app: {
                src: [scriptDir,
                    'Scripts/app/app.js',
                    'Scripts/app/util.js',
                    'Scripts/app/widgets/document-upload.js',
                    'Scripts/app/account/manage.js',
                    'Scripts/app/results/results.js'
                ],
                exclude: [],
                directives: {
                    vars: true,
                    browser: true,
                    predef: {
                        alert: false,
                        app: true,
                        $: false,
                        QUnit: false,
                        require: false,
                        google: false
                    }
                }
            }
        },
        qunit: {
            all: ['Scripts/test/**/*.html']
        },
        'jsmin-sourcemap': {
            app: {
                src: [
                    'Scripts/app/app.js',
                    'Scripts/app/util.js',
                    'Scripts/app/widgets/document-upload.js',
                    'Scripts/app/account/manage.js',
                    'Scripts/app/results/*.js'
                ],
                dest: 'wwwroot/dist/app/app.min.js',
                destMap: 'wwwroot/dist/app/app.js.map'

            },
            lib: {
                src: [
                      'wwwroot/lib/jquery/**/*.js',
                      'wwwroot/lib/jquery-validation/**/*.js',
                      'wwwroot/lib/jquery-validation-unobtrusive/**/*.js',
                      'wwwroot/lib/requirejs/**/*.js',
                      'wwwroot/lib/js/bootstrap/bootstrap.js',
                      'wwwroot/lib/bootstrap-contextmenu/bootstrap-contextmenu.js'
                ],
                dest: 'wwwroot/dist/lib/lib.min.js',
                destMap: 'wwwroot/dist/lib/lib.js.map'
            }
        },
        cssmin: {
            options: {
                shorthandCompacting: false,
                roundingPrecision: -1,
                sourceMap: true
            },
            app: {
                files: {
                    'wwwroot/dist/app/app.min.css': ['wwwroot/css/**/*.css']
                }
            },
            lib: {
                files: {
                    'wwwroot/dist/lib/lib.min.css': ['wwwroot/lib/**/*.css']
                }
            }
        },
        watch: {
            //bower: {
            //    files: ['bower.json'],
            //    tasks: ['bower:install', 'jsmin-sourcemap:lib']
            //},
            appjs: {
                files: [scriptDir],
                tasks: ['jslint:app', 'jsmin-sourcemap:app', 'qunit:all']
            },
            appcss: {
                files: ['wwwroot/css/results/results.css'],
                tasks: ['cssmin:app']
            },
            lib: {
                files: ['wwwroot/lib/**/*.js'],
                tasks: ['jsmin-sourcemap:lib']
            },
            test: {
                files: ['Scripts/app/util.js', 'Scripts/test/**/*.js'],
                tasks: ['qunit:all']
            }
        }
    });

    // This command registers the default task which installs bower packages into wwwroot/lib, and runs jslint.
    grunt.registerTask('default', ['bower:install', 'jslint:app', 'jsmin-sourcemap:app', 'jsmin-sourcemap:lib']);
    grunt.loadNpmTasks('grunt-bower-task');
    // The following lines loads the grunt plugins.
    // these lines needs to be at the end of this file.
    // cannot use an array or varargs to load tasks from multiple plugins here. 
    // It seems that loadNpmTasks is a singular command which loads task(s) for a single plugin.
    // This api is a bit counter intuitive in that invokations cannot be chained.
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-jsmin-sourcemap');
    grunt.loadNpmTasks('grunt-contrib-qunit');
    grunt.loadNpmTasks('grunt-jslint');

};