# Neuron
AI framework for building AI behaviours for unity engine powered games. Early stage prototype. Uses DI principles with unity's serialize reference feature. Supported version: 2020.3.3f1

#### Installation:
Add an entry in your manifest.json as follows:
```C#
"com.kaiyum.neuron": "https://github.com/kaiyumcg/Neuron.git"
```

Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git",
"com.kaiyum.editorutil": "https://github.com/kaiyumcg/EditorUtil.git"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.