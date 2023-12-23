import './App.css'
import XO from './components/xo/XO'
import Player from './components/player/Player'
function App() {
 

  return (
    <>
      <button className='start-game'>start new game</button>
      <div className='game-container'>
        <Player name='player 1' value='x' state={false} />
        <XO />
        <Player name='player 2' value='o' state={true}/>
      </div>
    </>
  )
}

export default App
