import './Tracker.css'
import React from 'react'
import Checker from '../core/Checker'
import {normalizeClassNames} from '../core/Utilities'

const MaxCols = 10

const TrackerTypes = Object.freeze({
	Circle: 'circle',
	Single: 'single',
	Multi: 'multi'
})

class Tracker extends React.Component
{
	render()
	{
		let checkers = null
		switch(this.props.type)
		{
			case TrackerTypes.Circle:
				checkers = this.generateCircleCheckers()
				break
			case TrackerTypes.Multi:
				checkers = this.generateMultiStatusCheckers()
				break
			default:
				checkers = this.generateSingleStatusCheckers()
		}
		
		//Default single row output
		let output = (<div className="checkerLine">{checkers}</div>)
		
		//If we need rows, render rows
		if(checkers.length > MaxCols)
		{
			let rows = []
			let items = []
			for(let i = 0; i < checkers.length; i++)
			{
				if(i % MaxCols === 0)
				{
					if(items.length > 0)
						rows.push((<div className="checkerLine" key={`${this.props.keyWord}Row-${items.length}`}>{items}</div>))
					items = []
				}
				items.push(checkers[i])
			}
			//Catch the last row
			if(items.length > 0)
				rows.push((<div className="checkerLine" key={`${this.props.keyWord}Row-${items.length}`}>{items}</div>))
			
			output = rows
		}
		
		return (
			<div className={normalizeClassNames(`tracker ${this.props.className ? this.props.className : ''}`)}>
				<p className="label">{this.props.label}</p>
				{output}
			</div>
		)
	}
	
	generateCircleCheckers()
	{
		let checkers = []
		for(let i = 0; i < this.props.max; i++)
		{
			checkers.push(<Checker type={Checker.Types.Circle} checked={this.props.value > i} clickHandler={() => this.props.changeHandler(i + 1)} />)
		}
		
		return checkers
	}
	
	generateSingleStatusCheckers()
	{
		let checkers = []
		for(let i = 0; i < this.props.max; i++)
		{
			checkers.push(<Checker type={Checker.Types.Line} lineStatus={this.props.spent > i ? Checker.LineStatus.Double : Checker.LineStatus.None} key={`${this.props.keyWord}-${i}`} clickHandler={() => this.props.changeHandler(i + 1)} />);
		}
		
		return checkers
	}
	
	generateMultiStatusCheckers()
	{
		let checkers = []
		for(let i = 0; i < this.props.max; i++)
		{
			let lineStatus = Checker.LineStatus.None
			
			if(this.props.values.length === 3)
			{
				if(i < this.props.values[2] + this.props.values[1] + this.props.values[0])
					lineStatus = Checker.LineStatus.Single
				if(i < this.props.values[2] + this.props.values[1])
					lineStatus = Checker.LineStatus.Double
				if(i < this.props.values[2])
					lineStatus = Checker.LineStatus.Triple
			}
			else
			{
				if(i < this.props.values[1] + this.props.values[0])
					lineStatus = Checker.LineStatus.Single
				if(i < this.props.values[1])
					lineStatus = Checker.LineStatus.Triple
			}
			
			checkers.push(<Checker type={Checker.Types.Line} lineStatus={lineStatus} key={`${this.props.keyWord}-${i}`} clickHandler={() => this.props.changeHandler(lineStatus)} />);
		}
		
		return checkers
	}
}

Tracker.Types = TrackerTypes

export default Tracker
