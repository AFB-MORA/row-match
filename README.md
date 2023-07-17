# Row Match

## Usage

Open MainScene and play.

Project should work in any orientationa and resolution.

## Project

### Unity Vesion: 

2021.3.13f1

### Packages:

* Zenject: for dependency injection.
* UniTask: for async/await funtions in unity.
* UniRx: for reactive programming.
* DOTween: for tween animations.

### Known Issues:

* Particle systems same material but multiple batches.
* Performance drop when loading scenes.
  * Zenejct binding spike.
* Performance drop when enabling popups.
  * GameObject.SetActive() spike.
* WarpTmpText Performance drop.
* Recycle view performance drop when scrolling
  * GameObject.SetActive() spike.
* Completed effect and MemoryPool.Spawn performance drop
  * GameObject.SetActive() spike.
