﻿declare module LASI.Results.LexicalContext {
    interface VerbalContext {
        subjects: number[];
        directObjects: number[];
        indirectObjects: number[];
    }
    interface RefencerContext {
        referredTo: number[];
    }
}
declare module LASI.Results {
}