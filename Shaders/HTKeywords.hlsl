#ifndef HT_KEYWORDS_INCLUDED
#define HT_KEYWORDS_INCLUDED

// Shade
#pragma shader_feature_local_fragment _HT_SHADE_MODE_POS_AND_BLUR _HT_SHADE_MODE_RAMP

#pragma shader_feature_local_fragment _HT_USE_FIRST_SHADE
#pragma shader_feature_local_fragment _HT_USE_FIRST_SHADE_MAP
#pragma shader_feature_local_fragment _HT_USE_EX_FIRST_SHADE
#pragma shader_feature_local_fragment _HT_USE_SECOND_SHADE
#pragma shader_feature_local_fragment _HT_USE_SECOND_SHADE_MAP

#pragma shader_feature_local_fragment _HT_USE_RAMP_SHADE

#pragma shader_feature_local_fragment _HT_USE_SHADE_CONTROL_MAP

// RimLight
#pragma shader_feature_local_fragment _HT_USE_RIM_LIGHT
#pragma shader_feature_local_fragment _HT_USE_RIM_LIGHT_MAP

// Emission
#pragma shader_feature_local_fragment _HT_USE_EMISSION
#pragma shader_feature_local_fragment _HT_USE_EMISSION_MAP
#pragma shader_feature_local_fragment _HT_OVERRIDE_EMISSION_COLOR

// MatCap
#pragma shader_feature_local_fragment _HT_USE_MAT_CAP
#pragma shader_feature_local_fragment _HT_USE_MAT_CAP_MASK

// Light
#pragma shader_feature_local_fragment _HT_USE_MAIN_LIGHT_COOKIE_AS_SHADE
#pragma shader_feature_local_fragment _HT_RECEIVE_GI

#endif
