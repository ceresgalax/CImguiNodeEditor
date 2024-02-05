cmake_minimum_required(VERSION 3.6)

project(cimgui_node_editor)

file(GLOB IMGUI_SOURCES imgui/*.cpp )
file(GLOB IMGUI_NODE_EDITOR_SOURCES imgui-node-editor/*.cpp )

add_library(cimgui_node_editor SHARED
    ${IMGUI_SOURCES}
    ${IMGUI_NODE_EDITOR_SOURCES}
    "cimgui_node_editor.cpp"
)

target_include_directories(cimgui_node_editor PRIVATE imgui imgui-node-editor)
target_compile_features(cimgui_node_editor PUBLIC cxx_std_14)
