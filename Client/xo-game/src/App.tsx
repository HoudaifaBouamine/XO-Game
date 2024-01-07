import './App.css'
import XO from './components/xo/XO'
import Player from './components/player/Player'
import { useEffect, useState } from 'react'


type Player={
  player_Id:number,
  name:string 
}


type Game = {
  game_Id: number;
  board: string;
  player1: Player;
  player2: Player | null;
  currentTurn: Player;
  winner: Player | null;
  isGameOver: boolean;
};
function App() {
  const [player,setPlayer]=useState<Player | null>(null)
  const [game,setGame]=useState<Game | null>(null)


  const createPlayer = async() =>{
    const response = await fetch('https://4a78-154-121-76-74.ngrok-free.app/players/new',{headers: new Headers({'ngrok-skip-browser-warning':'1'})})
    const player:Player = await response.json()
    setPlayer(player)
  }
  
  useEffect(() =>{
    createPlayer()
  },[])


  const startGame = async()=>{
    const response = await fetch(`https://4a78-154-121-76-74.ngrok-free.app/games/join/${player?.player_Id}`,{headers: new Headers({'ngrok-skip-browser-warning':'1'})})
    const game = await response.json()
    console.log('game id',game.game_Id)
    setGame(game)
  }
 

  return (
    <>
      <button className='start-game' onClick={()=>startGame()}>start new game</button>
      <div className='game-container'>
        <Player name='player 1' value='x' state={false} />
        <XO />
        <Player name='player 2' value='o' state={true}/>
      </div>
    </>
  )
}


export default App
