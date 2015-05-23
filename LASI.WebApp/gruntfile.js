/// <binding ProjectOpened='watch' />
// This file in the main entry point for defining grunt tasks and using grunt plugins.
// Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409

module.exports = function (grunt) {
    /**
     * prefixor factory
     * @param {string} pathPrefix - the prefix
     */
    function prefix(pathPrefix ) {
        return function (path) {
            return pathPrefix + path;
        };
    };
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: true
                }
            }
        },
        jshint: {
            app: {
                src: 'wwwroot/app/**/*.js',
                verbose: true,
                maxparams: 4,
                undef: true,
                unused: true
            },
            test: {
                src: 'wwwroot/test/**/*.js',
                verbose: true,
                maxparams: 4,
                undef: true,
                unused: true
            },
        },
        qunit: {
            all: ['wwwroot/test/**/*.html']
        },
        'jsmin-sourcemap': {
            lib: {
                src: [
                    'jquery/**/*.js',
                    'jquery-validation/**/*.js',
                    'jquery-validation-unobtrusive/**/*.js',
                    'bootstrap/js/bootstrap.js',
                    'bootstrap-contextmenu/bootstrap-contextmenu.js'
                ].map(prefix('wwwroot/lib/')),
                dest: 'wwwroot/dist/lib/lib.min.js'
            }
        },
        concat: {
            dist: {
                src: [
                    'LASI.js',
                    'utilities/log.js',
                    'debug-panel/debug-panel.js',
                    'utilities/augmentations.js',
                    'account/manage.js',
                    'results/context-menu-provider.js',
                    'results/result-chart-provider.js',
                    'widgets/document-upload.js',
                    'widgets/document-list.js',
                    'widgets/document-list-app/section.js',
                    'widgets/document-list-app/app.js',
                    'widgets/document-list-app/document-list-service-provider.js',
                    'widgets/document-list-app/documents-service.js',
                    'widgets/document-list-app/delete-document-modal-controller.js',
                    'widgets/document-list-app/results-service.js',
                    'widgets/document-list-app/document-list-menu-item.js',
                    'widgets/document-list-app/document-list-tabset-item.js',
                    'widgets/document-list-app/tasks-list-service-provider.js',
                    'widgets/document-list-app/list-controller.js'
                ].map(prefix('wwwroot/app/')),
                dest: 'wwwroot/dist/app/app.js',
            }, options: { sourceMap: true }
        },
        cssmin: {
            options: {
                shorthandCompacting: false,
                roundingPrecision: -1,
                sourceMap: true,//want this to be on but it causes an error
                verbose: true
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
            appjs: {
                files: ['wwwroot/app/**/*.js'],
                tasks: ['qunit:all', 'concat']
            },
            test: {
                files: ['wwwroot/test/**/*.js'],
                tasks: ['jshint:test', 'qunit:all']
            },
            libjs: {
                files: ['wwwroot/lib/**/*.js'],
                tasks: ['jsmin-sourcemap:lib']
            },
            appcss: {
                files: ['wwwroot/css/**'],
                tasks: ['cssmin:app']
            },
            libcss: {
                files: ['wwwroot/lib/**/*.css'],
                tasks: ['cssmin:lib']
            }

        }
    });

    // This command registers the default task which installs bower packages into wwwroot/lib.
    grunt.registerTask('default', ['bower:install']);

    // register an alias for qunit tests called 'test'.
    grunt.registerTask('test', ['qunit:all']);

    // The following lines loads the grunt plugins.
    // these lines needs to be at the end of this file.
    // cannot use an array or varargs to load tasks from multiple plugins here. 
    // It seems that loadNpmTasks is a singular command which loads task(s) for a single plugin.
    // This api is a bit counter intuitive in that invocations cannot be chained.
    grunt.loadNpmTasks('grunt-bower-task');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-jsmin-sourcemap');
    grunt.loadNpmTasks('grunt-contrib-qunit');
    grunt.loadNpmTasks('grunt-contrib-jshint');
};