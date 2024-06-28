import './Player.css'
import arrow from '../../assets/arrow.svg'
import { useState } from 'react'

type Props={
    name:string,
    value:string,
    state:boolean
}

const Player = ({name,value,state}:Props) => {
    const [turn,setTurn]=useState(state)
    return (
        <div className='player'>
            <img style={{visibility: turn ? 'hidden' : 'visible'}} src={arrow} alt={`${name} turn`} /> 
            <div>
                <p>{name}</p>
                <h1>{value}</h1>
            </div>
        </div>
  )
}

export default Player