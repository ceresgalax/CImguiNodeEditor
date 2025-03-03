cmake_minimum_required(VERSION 3.12)
include(GenerateExportHeader)

project(cimgui_node_editor CXX)

file(GLOB IMGUI_SOURCES cimgui/imgui/*.cpp )
file(GLOB IMGUI_NODE_EDITOR_SOURCES imgui-node-editor/*.cpp )
# file(GLOB IMGUI_NODE_EDITOR_BPEXAMPLE_SOURCES imgui-node-editor/examples/blueprints-example/utilities/*.cpp)

add_library(cimgui_node_editor SHARED
    ${IMGUI_SOURCES}
    ${IMGUI_NODE_EDITOR_SOURCES}
    # ${IMGUI_NODE_EDITOR_BPEXAMPLE_SOURCES}
    "imgui-node-editor/examples/blueprints-example/utilities/drawing.cpp"
    "imgui-node-editor/examples/blueprints-example/utilities/widgets.cpp"
    "cimgui_node_editor.cpp"
    "extra.cpp"
)



generate_export_header(cimgui_node_editor)

target_include_directories(cimgui_node_editor PRIVATE cimgui/imgui imgui-node-editor ${PROJECT_BINARY_DIR})
target_compile_features(cimgui_node_editor PUBLIC cxx_std_14)
target_compile_definitions(cimgui_node_editor PUBLIC IMGUI_DISABLE_OBSOLETE_FUNCTIONS=1)
target_compile_definitions(cimgui_node_editor PRIVATE cimgui_node_editor_EXPORTS)
set_target_properties(cimgui_node_editor PROPERTIES
    PREFIX ""
    OUTPUT_NAME "cimguinodeeditor"
)
